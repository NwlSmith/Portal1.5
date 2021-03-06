﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/*
 * Date created: 4/2/2020
 * Creator: Nate Smith
 * 
 * Description: Portal camera script.
 * Ensures the camera on each portal maintains visual coherence.
 */
public class PortalCamera : MonoBehaviour
{
    public MeshRenderer[] modelMRs;
    private Transform playerCameraTrans;
    private Portal parentPortal;
    private Camera cam;
    private Renderer[] modelRenderers;
    private RenderTexture rt;
    private RenderTexture prevRT;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        modelRenderers = new Renderer[modelMRs.Length];
        for (int i = 0; i < modelMRs.Length; i++)
            modelRenderers[i] = modelMRs[i].GetComponent<Renderer>();
    }

    private void Start()
    {
        playerCameraTrans = FindObjectOfType<PlayerLook>().transform;
        parentPortal = GetComponentInParent<Portal>();
        

        // Create a new RenderTexture so RT resolution stays the same across portals.
        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }
        rt = new RenderTexture(Screen.width, Screen.height, 24);
        prevRT = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = rt;

        // Pair with the other portal.
        if (parentPortal.Other() != null)
        {
            NewPairedPortal(parentPortal.Other().portalCamera);
            parentPortal.Other().portalCamera.NewPairedPortal(this);
        }
    }


    /*
     void Update()
     {
         // Retrieve the other portal.
         Portal otherPortal = parentPortal.Other();
         if (otherPortal != null)
         {
             // The relative local rotation of the player to the other portal's forward vector.
             TransformRotLoc(otherPortal.transform);
             // The relative local position of the player to the other portal's forward vector.
             TransformPosLoc(otherPortal.transform);
             // Set the nearest objects that can be rendered to those past the plane formed by the portal.
             CalculateProjectionMatrix();



         }
     }*/

    public void Render(int index, int maxNumRecursions)
    {
        Portal otherPortal = parentPortal.Other();
        if (otherPortal != null)
        {
            // The relative local position of the player to the other portal's forward vector.
            TransformPosLoc(otherPortal.transform);

            // The relative local rotation of the player to the other portal's forward vector.
            TransformRotLoc(otherPortal.transform);

            Vector3 adjustedRelativePos = parentPortal.transform.InverseTransformPoint(otherPortal.transform.position);
            Quaternion adjustedRelativeRot = parentPortal.transform.rotation * Quaternion.Inverse(otherPortal.transform.rotation * Quaternion.Euler(0, 180, 0));
            
            Vector3 originalLocalPos = transform.localPosition;
            Quaternion originalLocalRot = transform.localRotation;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            for (int i = 0; i < (maxNumRecursions - 1 - index); i++)
            {
                transform.rotation *= adjustedRelativeRot;
                transform.Translate(-adjustedRelativePos);
            }

            transform.Translate(originalLocalPos);
            transform.rotation *= originalLocalRot;

            // Set the nearest objects that can be rendered to those past the plane formed by the portal.
            CalculateProjectionMatrix();

            if (index == 0)
            {
                RenderTexture previousActive = RenderTexture.active;
                RenderTexture.active = rt;
                GL.Clear(true, true, Color.black);
                RenderTexture.active = previousActive;
            }
            cam.Render();

        }
    }

    /*
     * Sets the other portal's camera's RenderTexture to the images captured on this camera.
     * Called in Start().
     */
    public void NewPairedPortal(PortalCamera otherPortalCamera)
    {
        for (int i = 0; i < otherPortalCamera.modelMRs.Length; i++)
        {
            otherPortalCamera.modelMRs[i].material.mainTexture = rt;
        }
    }

    /*
     * Rotates the camera to the rotation of the player relative to the other portal.
     * Called in Update().
     */
    private void TransformRotLoc(Transform otherPortalTrans)
    {
        Quaternion localRot = Quaternion.Inverse(otherPortalTrans.rotation) * Camera.main.transform.rotation;
        transform.localRotation = localRot;
        transform.RotateAround(transform.position, parentPortal.transform.up, 180f);
    }

    /*
     * Transforms the camera at the position of the player relative to the other portal.
     * Called in Update().
     */
    private void TransformPosLoc(Transform otherPortalTrans)
    {
        Vector3 relativePos = otherPortalTrans.InverseTransformPoint(Camera.main.transform.position);
        relativePos = Vector3.Scale(relativePos, new Vector3(-1, 1, -1));
        transform.position = parentPortal.transform.TransformPoint(relativePos);
    }

    /*
     * Distort the near face of the camera so that it only begins rendering as soon as it passes the plane made by the portal.
     * Called in Update().
     */
    private void CalculateProjectionMatrix()
    {
        cam.projectionMatrix = Camera.main.projectionMatrix;
        Plane portalPlane = new Plane(parentPortal.transform.forward, parentPortal.transform.position);
        Vector4 clipPlane = new Vector4(portalPlane.normal.x, portalPlane.normal.y, portalPlane.normal.z, portalPlane.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * clipPlane;
        cam.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/2/2020
 * Creator: Nate Smith
 * 
 * Description: Portal camera script.
 * Ensures the camera on each portal maintains visual coherence.
 */
public class PortalCamera : MonoBehaviour
{
    public MeshRenderer modelMR;
    private Transform playerCameraTrans;
    private Portal parentPortal;
    private Camera cam;
    private RenderTexture rt;
    private RenderTexture prevRT;

    private void Start()
    {
        playerCameraTrans = FindObjectOfType<PlayerLook>().transform;
        parentPortal = GetComponentInParent<Portal>();

        cam = GetComponent<Camera>();

        // Create a new RenderTexture so RT resolution stays the same across portals.
        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }
        rt = new RenderTexture(Screen.width, Screen.height, 24);
        prevRT = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = rt;

        // Pair with the other portal.
        if (PortalManager.instance.OtherPortal(parentPortal) != null)
        {
            NewPairedPortal(PortalManager.instance.OtherPortal(parentPortal).portalCamera);
            PortalManager.instance.OtherPortal(parentPortal).portalCamera.NewPairedPortal(this);
        }
    }

    
    void Update()
    {
        // Retrieve the other portal.
        Portal otherPortal = PortalManager.instance.OtherPortal(parentPortal);
        if (otherPortal != null)
        {
            cam.projectionMatrix = Camera.main.projectionMatrix;
            // The relative local rotation of the player to the other portal's forward vector.
            TransformRotLoc(otherPortal.transform);
            // The relative local position of the player to the other portal's forward vector.
            TransformPosLoc(otherPortal.transform);
            // Set the nearest objects that can be rendered to those past the plane formed by the portal.
            CalculateProjectionMatrix();

            

        }
    }
    /*

    private void LateUpdate()
    {
        Graphics.Blit(rt, prevRT);
    }*/

    public void Render(int index, int maxNumRecursions)
    {
        Portal otherPortal = PortalManager.instance.OtherPortal(parentPortal);
        if (otherPortal != null)
        {
            cam.projectionMatrix = Camera.main.projectionMatrix;
            // The relative local rotation of the player to the other portal's forward vector.
            Quaternion localRot = Quaternion.Inverse(otherPortal.transform.rotation) * Camera.main.transform.rotation;
            Vector3 adjustedLocalRot = localRot.eulerAngles - new Vector3(0, -180, 0);
            transform.localRotation = Quaternion.Euler(adjustedLocalRot);

            // The relative local position of the player to the other portal's forward vector.
            Vector3 relativePos = otherPortal.transform.InverseTransformPoint(Camera.main.transform.position);
            Vector3 adjustedRelativePos = Vector3.Scale(relativePos, new Vector3(-1, 1, -1));
            transform.localPosition = adjustedRelativePos;

            for(int i = 0; i < (maxNumRecursions - 1 - index); i++)
            {
                transform.localRotation *= Quaternion.Euler(adjustedLocalRot);
                transform.localPosition += adjustedRelativePos;
            }

            Debug.Log("transform.localRot " + transform.localEulerAngles + "transform.localPos " + transform.localPosition);


            // Set the nearest objects that can be rendered to those past the plane formed by the portal.
            Plane portalPlane = new Plane(parentPortal.transform.forward, parentPortal.transform.position);
            Vector4 clipPlane = new Vector4(portalPlane.normal.x, portalPlane.normal.y, portalPlane.normal.z, portalPlane.distance);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * clipPlane;
            cam.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlaneCameraSpace);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = transform.position;
            cube.name = "EYYYYYYYYYYYYYYYYYYYYYY";

            if (index == 0)
            {
                RenderTexture previousActive = RenderTexture.active;
                RenderTexture.active = rt;
                GL.Clear(true, true, Color.red);
                RenderTexture.active = previousActive;
            }
            cam.Render();

            DestroyImmediate(cube);
        }
    }

    /*
     * Sets the other portal's camera's RenderTexture to the images captured on this camera.
     * Called in Start().
     */
    public void NewPairedPortal(PortalCamera otherPortalCamera)
    {
        otherPortalCamera.modelMR.material.mainTexture = rt;
    }

    /*
     * Rotates the camera to the rotation of the player relative to the other portal.
     * Called in Update().
     */
    private void TransformRotLoc(Transform otherPortalTrans)
    {
        Quaternion localRot = Quaternion.Inverse(otherPortalTrans.rotation) * Camera.main.transform.rotation;
        Vector3 relativeRot = localRot.eulerAngles - new Vector3(0, -180, 0);
        transform.localRotation = Quaternion.Euler(relativeRot);
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
        Plane portalPlane = new Plane(parentPortal.transform.forward, parentPortal.transform.position);
        Vector4 clipPlane = new Vector4(portalPlane.normal.x, portalPlane.normal.y, portalPlane.normal.z, portalPlane.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * clipPlane;
        cam.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }
}

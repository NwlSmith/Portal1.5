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

    private void Start()
    {
        playerCameraTrans = FindObjectOfType<PlayerLook>().transform;
        parentPortal = GetComponentInParent<Portal>();

        cam = GetComponent<Camera>();

        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }

        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

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
            // The relative local rotation of the player to the other portal's forward vector.
            TransformRotLoc(otherPortal.transform);
            // The relative local position of the player to the other portal's forward vector.
            TransformPosLoc(otherPortal.transform);
        }
    }

    public void NewPairedPortal(PortalCamera otherPortalCamera)
    {
        otherPortalCamera.modelMR.material.mainTexture = cam.targetTexture;
    }

    /*
     * Rotates the camera to the rotation of the player relative to the other portal.
     * Called in Update().
     */
    private void TransformRotLoc(Transform otherPortalTrans)
    {
        Vector3 relativeRot = otherPortalTrans.InverseTransformDirection(Camera.main.transform.forward);
        relativeRot = Vector3.Scale(relativeRot, new Vector3(-1, 1, -1));
        transform.forward = parentPortal.transform.TransformDirection(relativeRot);
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
}

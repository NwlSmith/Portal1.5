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
    private Transform playerCameraTrans;
    private Portal parentPortal;

    private void Start()
    {
        playerCameraTrans = FindObjectOfType<PlayerLook>().transform;
        parentPortal = GetComponentInParent<Portal>();

        GetComponent<Camera>().targetTexture.height = Screen.height;
        GetComponent<Camera>().targetTexture.width = Screen.width;
    }
    void Update()
    {
        // Retrieve the other portal.
        Transform otherPortalTrans = PortalManager.instance.OtherPortal(parentPortal).transform;

        if (Input.GetKeyDown(KeyCode.Escape))
            Debug.Break();

        // The relative local rotation of the player to the other portal's forward vector.
        TransformRotLoc(otherPortalTrans);
        // The relative local position of the player to the other portal's forward vector.
        TransformPosLoc(otherPortalTrans);
    }

    private void TransformRotLoc1(Transform otherPortalTrans)
    {
        // The angle of the relative rotations of each portal.
        float relativePortalAngle = Quaternion.Angle(parentPortal.transform.rotation, otherPortalTrans.rotation);
        // The angle of the relative rotations of each portal in quaternion form. (May need to change parentPortal.transform.up to Vector3.up)
        Quaternion relativePortalQuat = Quaternion.AngleAxis(relativePortalAngle, parentPortal.transform.up);
        // Rotate the camera.
        transform.localRotation = Quaternion.LookRotation(relativePortalQuat * playerCameraTrans.forward, parentPortal.transform.up);
        //transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, 180, 0));
    }

    private void TransformRotLoc2(Transform otherPortalTrans)
    {
        // Vector3.Reflect(playerCameraTrans.forward,)
        
        Vector3 relativePortalAngle = parentPortal.transform.rotation.eulerAngles - playerCameraTrans.rotation.eulerAngles;
        transform.localRotation = Quaternion.LookRotation(Quaternion.Euler(relativePortalAngle) * playerCameraTrans.forward, parentPortal.transform.up);
    }

    private void TransformRotLoc3(Transform otherPortalTrans)
    {
        // The angle of the relative rotations of each portal.
        float relativePortalAngle = Quaternion.Angle(otherPortalTrans.rotation, parentPortal.transform.rotation);
        // The angle of the relative rotations of each portal in quaternion form. (May need to change parentPortal.transform.up to Vector3.up)
        Quaternion relativePortalQuat = Quaternion.AngleAxis(relativePortalAngle, parentPortal.transform.up);
        // Rotate the camera.
        transform.localRotation = Quaternion.LookRotation(relativePortalQuat * playerCameraTrans.forward, parentPortal.transform.up);
    }

    private void TransformRotLoc4(Transform otherPortalTrans)
    {
        // Rotate the camera.
        transform.localRotation = playerCameraTrans.rotation;
    }

    private void TransformRotLoc5(Transform otherPortalTrans)
    {
        // The angle of the relative rotations of each portal.
        float relativePortalAngle = Quaternion.Angle(parentPortal.transform.rotation, otherPortalTrans.rotation);
        // The angle of the relative rotations of each portal in quaternion form. (May need to change parentPortal.transform.up to Vector3.up)
        Quaternion relativePortalQuat = Quaternion.AngleAxis(relativePortalAngle, parentPortal.transform.up);
        // Rotate the camera.
        transform.localRotation = Quaternion.LookRotation(relativePortalQuat * playerCameraTrans.forward, otherPortalTrans.up);
    }

    private void TransformRotLoc(Transform otherPortalTrans)
    {
        Vector3 relativeRot = otherPortalTrans.InverseTransformDirection(Camera.main.transform.forward);
        relativeRot = Vector3.Scale(relativeRot, new Vector3(-1, 1, -1));
        transform.forward = parentPortal.transform.TransformDirection(relativeRot);
    }


    private void TransformPosLoc1(Transform otherPortalTrans)
    {
        // Move the camera to correspond to the difference between the player's position and the other portal's position.
        transform.localPosition = playerCameraTrans.position - otherPortalTrans.position;
    }

    private void TransformPosLoc2(Transform otherPortalTrans)
    {
        // Move the camera to correspond to the difference between the player's position and the other portal's position.
        Vector3 tempVec3 = playerCameraTrans.position - otherPortalTrans.position;
        transform.localPosition = parentPortal.transform.rotation * tempVec3;
    }

    private void TransformPosLoc(Transform otherPortalTrans)
    {
        Vector3 relativePos = otherPortalTrans.InverseTransformPoint(Camera.main.transform.position);
        relativePos = Vector3.Scale(relativePos, new Vector3(-1, 1, -1));
        transform.position = parentPortal.transform.TransformPoint(relativePos);
    }


    private void TransformRotGlo(Transform otherPortalTrans)
    {
        // The angle of the relative rotations of each portal.
        float relativePortalAngle = Quaternion.Angle(parentPortal.transform.rotation, otherPortalTrans.rotation);
        // The angle of the relative rotations of each portal in quaternion form. (May need to change parentPortal.transform.up to Vector3.up)
        Quaternion relativePortalQuat = Quaternion.AngleAxis(relativePortalAngle, parentPortal.transform.up);
        // Rotate the camera.
        transform.rotation = Quaternion.LookRotation(relativePortalQuat * playerCameraTrans.forward, parentPortal.transform.up);
        transform.Rotate(2*transform.rotation.eulerAngles.x, 180, 0);
    }

    private void TransformPosGlo(Transform otherPortalTrans)
    {
        // Move the camera to correspond to the difference between the player's position and the other portal's position.
        transform.position = parentPortal.transform.position + playerCameraTrans.position - otherPortalTrans.position;
    }
}

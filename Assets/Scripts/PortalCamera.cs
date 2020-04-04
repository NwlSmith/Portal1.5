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

        // The angle of the relative rotations of each portal.
        float relativePortalAngle = Quaternion.Angle(parentPortal.transform.rotation, otherPortalTrans.rotation);
        // The angle of the relative rotations of each portal in quaternion form. (May need to change parentPortal.transform.up to Vector3.up)
        Quaternion relativePortalQuat = Quaternion.AngleAxis(relativePortalAngle, parentPortal.transform.up);
        // Rotate the camera.
        // Vector3.Reflect(playerCameraTrans.forward, playerCameraTrans.right)
        transform.rotation = Quaternion.LookRotation(relativePortalQuat * playerCameraTrans.forward, playerCameraTrans.up);
        transform.Rotate(2*transform.rotation.eulerAngles.x, 180, 0);

        // Move the camera to correspond to the difference between the player's position and the other portal's position.
        transform.position = parentPortal.transform.position + playerCameraTrans.position - otherPortalTrans.position;
    }
}

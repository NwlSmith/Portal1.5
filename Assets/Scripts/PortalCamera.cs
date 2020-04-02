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
    private Transform playerTrans;
    private Portal parentPortal;

    private void Start()
    {
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
        parentPortal = GetComponentInParent<Portal>();
    }
    void Update()
    {
        // Rotate the camera in relation to the player.
        Vector3 dirTransformVector = Quaternion.LookRotation(parentPortal.transform.forward - (transform.position - playerTrans.position).normalized).eulerAngles;
        transform.rotation = Quaternion.Euler(dirTransformVector);

        // Move the camera to correspond to the difference between the player's position and the other portal's position.
        transform.position = playerTrans.position - PortalManager.instance.OtherPortal(parentPortal).transform.position;
    }
}

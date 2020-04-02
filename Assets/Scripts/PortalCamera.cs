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
        Vector3 dirTransformVector =
            (PortalManager.instance.OtherPortal(parentPortal).transform.rotation.eulerAngles
            + new Vector3(0, 180, 0) - playerTrans.transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Euler(dirTransformVector);
    }
}

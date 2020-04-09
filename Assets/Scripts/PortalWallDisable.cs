using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/7/2020
 * Creator: Nate Smith
 * 
 * Description: Disables and enables wall collider when Player enters and leaves collider.
 * Found on PortalWallDisableCollider GameObject
 */
public class PortalWallDisable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by " + other.name);
        if (other.tag == "Player")
        {
            Debug.Log("Portal wall disable entered by " + other.name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
        }
        if (other.tag == "CanPickUp")
        {
            Debug.Log("Portal entered by " + other.name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Portal " + name + " exited by " + other.name);
        if (other.tag == "Player")
        {
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = true;
            PortalManager.instance.OtherPortal(GetComponentInParent<Portal>()).surface.GetComponent<Collider>().enabled = true;
        }
        if (other.tag == "CanPickUp")
        {
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = true;
            PortalManager.instance.OtherPortal(GetComponentInParent<Portal>()).surface.GetComponent<Collider>().enabled = true;
        }
    }
}

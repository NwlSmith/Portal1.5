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
        if (other.tag == "Player")
        {
            Debug.Log("Portal entered by " + name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
        }
        if (other.tag == "CanPickUp")
        {
            Debug.Log("Portal entered by " + name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = true;
        }
    }
}

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
    private int portalLayer;

    private void Start()
    {
        // Stores the layer that this object should set the player to should they enter the trigger.
        portalLayer = GetComponentInParent<Portal>().blue ? 12 : 13;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by " + other.name);
        if (other.tag == "Player")
        {
            Debug.Log("Portal wall disable entered by " + other.name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            //GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
            StopCollidingWithPortalSurface(other.gameObject);
        }
        if (other.tag == "CanPickUp")
        {
            Debug.Log("Portal entered by " + other.name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            //GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
            StopCollidingWithPortalSurface(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Portalwalldisabler of " + transform.parent.name + " exited by " + other.name);
        if (other.tag == "Player")
        {
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = true;
            //PortalManager.instance.OtherPortal(GetComponentInParent<Portal>()).surface.GetComponent<Collider>().enabled = true;
            StartCollidingWithPortalSurface(other.gameObject);
        }
        if (other.tag == "CanPickUp")
        {
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = true;
            //PortalManager.instance.OtherPortal(GetComponentInParent<Portal>()).surface.GetComponent<Collider>().enabled = true;
            StartCollidingWithPortalSurface(other.gameObject);
        }
    }

    private void StopCollidingWithPortalSurface(GameObject go)
    {
        int otherLayer = GetComponentInParent<Portal>().blue ? 13 : 12;
        // If the object is already set to not collide with the other portal's surface collider
        if (go.layer == otherLayer)
            // don't collide with either
            go.layer = 14;
        else
            go.layer = portalLayer;
    }

    public void StartCollidingWithPortalSurface(GameObject go)
    {
        int otherLayer = GetComponentInParent<Portal>().blue ? 13 : 12;
        // If the object is already also set to not collide with the other portal's surface collider
        if (go.layer == 14)
            // Still don't collide with them
            go.layer = otherLayer;
        else
        {
            if (go.tag == "Player")
                go.layer = 0;
            else if (go.tag == "CanPickUp")
                go.layer = 10;
        }
            
    }
}

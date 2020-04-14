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
    private Portal parentPortal;
    private int portalLayer;


    private void Start()
    {
        // Stores the portal that this PortalWallDisable is a part of.
        parentPortal = GetComponentInParent<Portal>();

        // Stores the layer that this object should set the player to should they enter the trigger.
        portalLayer = parentPortal.blue ? 12 : 13;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by " + other.name + " on layer " + other.gameObject.layer);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Portal wall disable entered by " + other.name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            //GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
            StopCollidingWithPortalSurface(other.gameObject);
        }
        if (other.CompareTag("CanPickUp"))
        {
            Debug.Log("Portal entered by " + other.name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            //GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
            StopCollidingWithPortalSurface(other.transform.parent.gameObject);
            other.GetComponentInParent<ObjectUtility>().enteredPortal = parentPortal;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Portalwalldisabler of " + transform.parent.name + " exited by " + other.name + " on layer " + other.gameObject.layer);
        if (other.CompareTag("Player"))
        {
            //PortalManager.instance.OtherPortal(GetComponentInParent<Portal>()).surface.GetComponent<Collider>().enabled = true;
            StartCollidingWithPortalSurface(other.gameObject);
        }
        if (other.CompareTag("CanPickUp"))
        {
            //PortalManager.instance.OtherPortal(GetComponentInParent<Portal>()).surface.GetComponent<Collider>().enabled = true;
            StartCollidingWithPortalSurface(other.transform.parent.gameObject);
            other.GetComponentInParent<ObjectUtility>().enteredPortal = null;
        }
    }

    public void StopCollidingWithPortalSurface(GameObject go)
    {
        int otherLayer = parentPortal.blue ? 13 : 12;
        // If the object is already set to not collide with the other portal's surface collider
        if (go.layer == otherLayer)
            // don't collide with either
            go.layer = 14;
        else
            go.layer = portalLayer;
    }

    public void StartCollidingWithPortalSurface(GameObject go)
    {
        int otherLayer = parentPortal.blue ? 13 : 12;
        // If the object is already also set to not collide with the other portal's surface collider
        if (go.layer == 14)
            // Still don't collide with them
            go.layer = otherLayer;
        else
        {
            if (go.tag == "Player")
                go.layer = 21;
            else if (go.tag == "CanPickUp")
                go.layer = 20;
        }
    }
}

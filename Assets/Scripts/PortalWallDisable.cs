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

    /*
     * React to a collider entering the trigger, meaning it is close to the portal.
     * If it is an appropriate object, alters the object collisions.
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by " + other.name + " on layer " + other.gameObject.layer);
        // If the player is entering the trigger...
        if (other.CompareTag("Player"))
        {
            Debug.Log("Portal wall disable entered by " + other.name);
            // Mark it as NOT colliding with this surface.
            StopCollidingWithPortalSurface(other.gameObject);
        }
        // If an object is entering the trigger...
        if (other.CompareTag("CanPickUp") && other.gameObject.layer == 10)
        {
            Debug.Log("Portal entered by " + other.name);
            // Mark it as NOT colliding with this surface.
            StopCollidingWithPortalSurface(other.transform.parent.gameObject);
            // Make sure the clone object tracks to this portal.
            other.GetComponentInParent<ObjectUtility>().enteredPortal = parentPortal;
        }
    }

    /*
     * React to a collider leaving the trigger, meaning it is not close to the portal.
     * If it is an appropriate object, alters the object collisions.
     */
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Portalwalldisabler of " + transform.parent.name + " exited by " + other.name + " on layer " + other.gameObject.layer);
        // If the player is exiting the trigger...
        if (other.CompareTag("Player"))
        {
            // Mark it as colliding with this surface.
            StartCollidingWithPortalSurface(other.gameObject);
        }
        // If an object is exiting the trigger...
        if (other.CompareTag("CanPickUp") && other.gameObject.layer == 10)
        {
            // Mark it as colliding with this surface.
            StartCollidingWithPortalSurface(other.transform.parent.gameObject);
            // Make sure the clone object does not track to any portal.
            other.GetComponentInParent<ObjectUtility>().enteredPortal = null;
        }
    }

    public void Failsafe()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.layer == portalLayer || player.layer == 14)
            StartCollidingWithPortalSurface(player);

        StartCollidingWithPortalSurface(player);
        GameObject[] pickupables = GameObject.FindGameObjectsWithTag("CanPickUp");
        foreach (GameObject obj in pickupables)
        {
            if (obj.layer == portalLayer || obj.layer == 14)
                StartCollidingWithPortalSurface(obj);
        }
    }

    /*
     * Stops the object from colliding with certain physics layers.
     * The game needs to prevent the object from colliding with the surface the portal is on
     * so that the object passes through it and teleports.
     * Called in TeleportObject() in Portal.cs and OnTriggerEnter().
     */
    public void StopCollidingWithPortalSurface(GameObject go)
    {
        // Determine what is the no-collision layer number of the other portal to set the player/object to.
        // 0 is none, 12 is blue, 13 is orange, 14 is both
        // If set to 12, the player will NOT collide with the surface the blue portal is on.
        int otherLayer = parentPortal.blue ? 13 : 12;
        // If the object is already set to not collide with the other portal's surface collider...
        if (go.layer == otherLayer)
            // Don't collide with either.
            go.layer = 14;
        else
            // Otherwise, don't collide with this portal's surface.
            go.layer = portalLayer;
    }

    /*
     * Starts the object colliding with certain physics layers again.
     * The game needs to allow the object to collide with the surface the portal is on
     * after teleporting, otherwise objects will pass through walls/floors.
     * Called in TeleportPlayer() and TeleportObject() in Portal.cs and OnTriggerExit().
     */
    public void StartCollidingWithPortalSurface(GameObject go)
    {
        // Determine what is the no-collision layer number of the other portal to set the player/object to.
        int otherLayer = parentPortal.blue ? 13 : 12;
        // If the object is already ALSO set to not collide with the other portal's surface collider, ie, both...
        if (go.layer == 14)
            // Still don't collide with the other collider
            go.layer = otherLayer;
        else
        {
            // Otherwise, either set the object to the player's layer or the object layer
            if (go.CompareTag("Player"))
                go.layer = 21;
            else if (go.CompareTag("CanPickUp"))
                go.layer = 20;
        }
    }
}

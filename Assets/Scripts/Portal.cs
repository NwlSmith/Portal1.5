using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/1/2020
 * Creator: Nate Smith
 * 
 * Description: Portal script.
 * Any objects entering the collider will be teleported to the other portal.
 */
public class Portal : MonoBehaviour
{
    public bool blue;
    public GameObject surface;
    public PortalCamera portalCamera;
    private int cullingLayer;

    private void Start()
    {
        portalCamera = GetComponentInChildren<PortalCamera>();

        // Update the PortalManager.
        if (blue)
        {
            if (PortalManager.instance.blue != null)
                PortalManager.instance.blue.DestroyMe();
            PortalManager.instance.blue = this;
        }
        else
        {
            if (PortalManager.instance.orange != null)
                PortalManager.instance.orange.DestroyMe();
            PortalManager.instance.orange = this;
        }

        // Set the layer culling mask to either the blue culling mask or the orange culling mask.
        // Culling masks basically say "render this" or "don't render this".
        cullingLayer = blue ? 12 : 13;

        // Set the surface to be culled by this camera.
        //CullingMaskCreate();
    }

    /*
     * Add this portal's culling to the surface.
     * Called in Start().
     */
    private void CullingMaskCreate()
    {
        // Check if surface is being culled by other portal, ie, if both portals on the same object.
        int otherLayer = cullingLayer == 12 ? 13 : 12;
        if (surface.layer == otherLayer || surface.layer == 14)
        {
            // Set the surface to be culled by both.
            surface.layer = 14;
        } else
        {
            // Otherwise, make surface be culled by this portal.
            surface.layer = cullingLayer;
        }
    }

    /*
     * Remove this portal's culling from the surface.
     * Called in DestroyMe().
     */
    private void CullingMaskRemove()
    {
        // Check if surface is being culled by both portals.
        if (surface.layer == 14)
        {
            // Set the surface to be culled by the other portal.
            surface.layer = cullingLayer == 12 ? 13 : 12;
        }
        else
        {
            // Otherwise, make surface be culled by neither portal.
            surface.layer = 0;
        }
    }

    /*
     * Teleports gameObjects, either the player or a object that can be picked up, to the other portal
     * Called when a collider enters the trigger collider.
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered trigger...");
        if (other.tag == "MainCamera")
        {
            Debug.Log("Camera entered trigger");
            // Check if the player is moving into the portal.
            if (other.GetComponentInParent<PlayerMovement>().VelocityCheck(transform.forward))
                // Teleport the player.
                TeleportPlayer(other.GetComponentInParent<PlayerMovement>());
        }

        if (other.tag == "Player")
        {
            Debug.Log("Player entered trigger on " + gameObject.name + " at " + transform.position);
            // Check if the player is moving into the portal.
            if (other.GetComponent<PlayerMovement>().VelocityCheck(transform.forward))
                // Teleport the player.
                TeleportPlayer(other.GetComponent<PlayerMovement>());
        }

        if (other.tag == "CanPickUp")
        {
            Debug.Log("Object " + other.name + " entered trigger on " + gameObject.name + " at " + transform.position);
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            // Check if the object is moving into the portal.
            if (otherRB.VelocityCheck(transform.forward))
                // Teleport the object.
                TeleportObject(otherRB);
        }
    }

    /*
     * Teleports the player to the other portal.
     * Called in OnTriggerEnter().
     */
    public void TeleportPlayer(PlayerMovement playerMovement)
    {
        Debug.Log("Teleported Player.");
        playerMovement.TeleportPlayer(transform, PortalManager.instance.OtherPortal(this).transform);
        surface.GetComponent<Collider>().enabled = true;
    }

    /*
     * Teleports the object to the other portal.
     * CURRENTLY CAUSES PLAYER TO FALL THROUGH WORLD.
     * Called in OnTriggerEnter().
     */
    public void TeleportObject(Rigidbody otherRB)
    {
        // If the player is carrying the object, drop it.
        PickupObject po = FindObjectOfType<PickupObject>();
        if (po.carriedObject == otherRB.gameObject)
            po.dropObject();

        // THEN Teleport it.
        Debug.Log("Teleported object" + otherRB.name);
        otherRB.TeleportObject(transform, PortalManager.instance.OtherPortal(this).transform);
        surface.GetComponent<Collider>().enabled = true;
    }

    /*
     * Destroys portal gameobject.
     * Plays animation, sound, and destroys gameobject after delay.
     * Called in DestroyPortals() and NewPortal() in PortalManager.cs.
     */
    public void DestroyMe()
    {
        GetComponent<Animator>().SetTrigger("Destroy");
        CullingMaskRemove();
        Destroy(gameObject, .15f);
    }
}

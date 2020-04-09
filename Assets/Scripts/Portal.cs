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

        SetSurfaceLayer();
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
        GetComponentInChildren<PortalWallDisable>().StartCollidingWithPortalSurface(playerMovement.gameObject);
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
        GetComponentInChildren<PortalWallDisable>().StartCollidingWithPortalSurface(otherRB.gameObject);
    }

    /*
     * Destroys portal gameobject.
     * Plays animation, sound, and destroys gameobject after delay.
     * Called in DestroyPortals() and NewPortal() in PortalManager.cs.
     */
    public void DestroyMe()
    {
        GetComponent<Animator>().SetTrigger("Destroy");
        ResetSurfaceLayer();
        Destroy(gameObject, .15f);
    }

    private void SetSurfaceLayer()
    {
        int otherLayer = blue ? 16 : 15;
        if (surface.layer == otherLayer)
            surface.layer = 17;
        else
            surface.layer = blue ? 15 : 16;
    }

    private void ResetSurfaceLayer()
    {
        int otherLayer = blue ? 16 : 15;
        if (surface.layer == 17)
            surface.layer = otherLayer;
        else
            surface.layer = 0;
    }
}

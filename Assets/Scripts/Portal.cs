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
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered trigger...");
        if (other.tag == "MainCamera")
        {
            Debug.Log("Camera entered trigger");
            TeleportPlayer(other.GetComponentInParent<PlayerMovement>());
        }

        if (other.tag == "Player")
        {
            Debug.Log("Player entered trigger on " + gameObject.name + " at " + transform.position);
            if (other.GetComponent<PlayerMovement>().VelocityCheck(transform.forward))
                TeleportPlayer(other.GetComponent<PlayerMovement>());
        }

        if (other.tag == "CanPickUp")
        {
            Debug.Log("Object " + other.name + " entered trigger on " + gameObject.name + " at " + transform.position);
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            if (otherRB.VelocityCheck(transform.forward))
                TeleportObject(otherRB);
        }
    }

    /*
     * Checks if the given object is entering the portal.
     * If the velocity of the object dotted with the normal vector of the portal is less than 0,
     * meaning they are roughly opposite, return true.
     * Called in OnTriggerEnter() in Portal.cs.
     */
    public bool ObjectVelocityCheck(GameObject obj)
    {
        if (Vector3.Dot(obj.GetComponent<Rigidbody>().velocity.normalized, transform.forward) < 0f)
            return true;
        return false;
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
        /* // NEED TO GET ISABELLA TO MAKE THESE THINGS PUBLIC
        PickupObject po = FindObjectOfType<PickupObject>();
        if (po.carriedObject == otherRB.gameObject)
            po.dropObject();
        */

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
        Destroy(gameObject, .15f);
    }
}

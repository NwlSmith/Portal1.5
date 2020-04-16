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
    [HideInInspector] public GameObject surface;
    [HideInInspector] public PortalCamera portalCamera;
    public AudioClip portalCreation;
    public AudioClip portalTraversal;

    private AudioSource audioSource;

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

        // Retrieve the audio source.
        if (!TryGetComponent(out audioSource))
        {
            Debug.Log("ERROR: object " + name + " created without AudioSource.");
        }
        else
        {
            audioSource.clip = portalCreation;
            audioSource.Play();
        }
    }

    /*
     * Teleports gameObjects, either the player or a object that can be picked up, to the other portal
     * Called when a collider enters the trigger collider.
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered trigger...");
        if (other.CompareTag("MainCamera") || other.CompareTag("CanPickUp"))
        {
            // Play teleportation sound.
            audioSource.pitch = Random.Range(.95f, 1.05f);
            audioSource.clip = portalTraversal;
            audioSource.Play();

            if (other.CompareTag("MainCamera"))
            {
                Debug.Log("Camera entered trigger");
                // Check if the player is moving into the portal.
                if (other.GetComponentInParent<PlayerMovement>().VelocityCheck(transform.forward))
                {
                    // Teleport the player.
                    TeleportPlayer(other.GetComponentInParent<PlayerMovement>());
                }
            }

            if (other.CompareTag("CanPickUp"))
            {
                Debug.Log("Object " + other.name + " entered trigger on " + gameObject.name);
                Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
                // Check if the object is moving into the portal.
                if (otherRB.VelocityCheck(transform.forward))
                {
                    // Teleport the object.
                    TeleportObject(otherRB);
                }
            }
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
        PortalManager.instance.OtherPortal(this).GetComponentInChildren<PortalWallDisable>().StopCollidingWithPortalSurface(otherRB.gameObject);
        otherRB.GetComponent<ObjectUtility>().enteredPortal = PortalManager.instance.OtherPortal(this);
    }

    /*
     * Destroys portal gameobject.
     * Plays animation, sound, and destroys gameobject after delay.
     * Called in DestroyPortals() and NewPortal() in PortalManager.cs.
     */
    public void DestroyMe()
    {
        GetComponent<Animator>().SetTrigger("Destroy");
        GetComponentInChildren<PortalWallDisable>().Failsafe();

        ResetSurfaceLayer();
        Destroy(gameObject, .15f);
    }

    /*
     * Sets the physics layer of the surface the portal is on.
     * The game needs to know which surface the portal is on so that it can turn off collisions between
     * the surface and the player when it gets close.
     * Marks the surface as holding a blue portal, an orange portal, or both.
     * Called in Start().
     */
    private void SetSurfaceLayer()
    {
        // Determine what is the layer number of the other portal
        // 0 is none, 15 is blue, 16 is orange, 17 is both
        int otherLayer = blue ? 16 : 15;
        // If the surface is already marked as containing the orange portal...
        if (surface.layer == otherLayer)
            // Mark it as containing both blue and orange
            surface.layer = 17;
        else
            // Otherwise mark it as containing this portal's color portal.
            surface.layer = blue ? 15 : 16;
    }

    /*
     * Resets the physics layer of the surface the portal is on.
     * The game needs to know which surface the portal is on so that it can turn off collisions between
     * the surface and the player when it gets close.
     * Called in DestroyMe().
     */
    private void ResetSurfaceLayer()
    {
        // Determine what is the layer number of the other portal
        int otherLayer = blue ? 16 : 15;
        // If the surface is already marked as containing both portals...
        if (surface.layer == 17)
            // Mark it as only containing the other portal
            surface.layer = otherLayer;
        else
            // Otherwise mark it as containing no portals.
            surface.layer = 0;
    }
}

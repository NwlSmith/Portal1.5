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
            Debug.Log("Player entered trigger");
            if (other.GetComponent<PlayerMovement>().VelocityCheck(transform.forward))
                TeleportPlayer(other.GetComponent<PlayerMovement>());
        }
    }

    /*
     * Teleports the player teleport function to the other portal.
     * Called in OnTriggerEnter().
     */
    public void TeleportPlayer(PlayerMovement playerMovement)
    {
        Debug.Log("Teleported Player.");
        playerMovement.TeleportPlayer(transform, PortalManager.instance.OtherPortal(this).transform);
    }

    /*
     * Destroys portal gameobject.
     * Plays animation, sound, and destroys gameobject after delay.
     * Called in DestroyPortals() and NewPortal() in PortalManager.cs.
     */
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}

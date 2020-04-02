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
        Debug.Log("Something entered trigger...");
        if (other.tag == "Player")
        {
            Debug.Log("Player entered trigger");
            TeleportPlayer(other.GetComponent<PlayerMovement>());
        }
    }

    /*
     * Teleports the player to the other portal.
     * Called in DestroyPortals() and NewPortal() in PortalManager.cs.
     */
    public void TeleportPlayer(PlayerMovement playerMovement)
    {
        Debug.Log("Teleported Player.");

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

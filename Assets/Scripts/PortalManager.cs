using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/1/2020
 * Creator: Nate Smith
 * 
 * Description: Portal manager script.
 * This a single instance static object - There should only be 1 PortalManager.
 * Manages and controls portals and associated utility functions.
 */
public class PortalManager : MonoBehaviour
{
    // Static instance of the object.
    public static PortalManager instance = null;

    // Public Variables.
    public Portal blue;
    public Portal orange;

    public GameObject bluePrefab;
    public GameObject orangePrefab;

    private void Awake()
    {
        // Ensure that there is only one instance of the PortalManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    /*
     * Returns the other portal.
     */
    public Portal OtherPortal(Portal p)
    {
        if (p == blue)
            return orange;
        else
            return blue;
    }

    /*
     * Destroys both portal gameobjects.
     * Called on player death and level-end functions.
     */
    public void DestroyPortals()
    {
        blue.DestroyMe();
        blue = null;

        orange.DestroyMe();
        orange = null;
    }
}

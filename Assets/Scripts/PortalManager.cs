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
    public int maxNumRecursions = 4;
    [HideInInspector] public Portal blue;
    [HideInInspector] public Portal orange;

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
    private void LateUpdate()
    {

        PortalCamera pcb1 = blue.portalCamera;
        PortalCamera pco1 = orange.portalCamera;
        PortalCamera pcb2 = pcb1.GetComponentInChildren<PortalCamera>();
        PortalCamera pco2 = pco1.GetComponentInChildren<PortalCamera>();
        PortalCamera pcb3 = pcb2.GetComponentInChildren<PortalCamera>();
        PortalCamera pco3 = pco2.GetComponentInChildren<PortalCamera>();
        PortalCamera pcb4 = pcb3.GetComponentInChildren<PortalCamera>();
        PortalCamera pco4 = pco3.GetComponentInChildren<PortalCamera>();
        pcb4.Render(3, maxNumRecursions);
        pco4.Render(3, maxNumRecursions);
        pcb3.Render(2, maxNumRecursions);
        pco3.Render(2, maxNumRecursions);
        pcb2.Render(1, maxNumRecursions);
        pco2.Render(1, maxNumRecursions);
        pcb1.Render(0, maxNumRecursions);
        pco1.Render(0, maxNumRecursions);
    }*/

    /*private void LateUpdate()
    {
        for (int r = 0; r < maxNumRecursions; r++)
        {
            PortalCamera pc
            for (int i = 0)
                blue.portalCamera.Render(r, maxNumRecursions);
            orange.portalCamera.Render(r, maxNumRecursions);
        }
    }*/

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
        if (blue != null)
        {
            blue.DestroyMe();
            blue = null;
            portalShooting.shotBlue = false;
        }

        if (orange != null)
        {
            orange.DestroyMe();
            orange = null;
            portalShooting.shotOrange = false;
        }
    }
}
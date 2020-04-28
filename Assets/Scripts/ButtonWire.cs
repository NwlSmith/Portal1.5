using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/28/2020
 * Creator: Nate Smith
 * 
 * Description: A material manager script for the "wires" extending from a button to a door.
 * Changes textures of objects when buttons are pressed or un-pressed.
 */
public class ButtonWire : MonoBehaviour
{
    // Public variables.

    // Off material.
    public Material off;
    // On material.
    public Material on;
    // The renderer that displays those materials.
    public Renderer r;

    private void Start()
    {
        r = transform.GetChild(0).GetComponent<Renderer>();
        TurnOff();
    }

    /*
     * Changes the material to its "on" version.
     * Called in ActivateButton() in ButtonController.cs.
     */
    public void TurnOn()
    {
        r.material = on;
    }

    /*
     * Changes the material to its "off" version.
     * Called in DeactivateButton() in ButtonController.cs.
     */
    public void TurnOff()
    {
        r.material = off;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/28/2020
 * Creator: Nate Smith
 * 
 * Description: Manages the many ButtonWire objects of this Button.
 * Attach to Button.
 * Call GetComponent<ButtonController>().ActivateButton() to turn all ButtonWires to their "On" material.
 * Call GetComponent<ButtonController>().DeactivateButton() to turn all ButtonWires to their "Off" material.
 */
public class ButtonController : MonoBehaviour
{
    public ButtonWire[] buttonWires;

    /*
     * Changes the materials in each of the buttonWires to their "on" versions.
     * Called in ?????.
     */
    public void ActivateButton()
    {
        foreach (ButtonWire buttonWire in buttonWires)
        {
            buttonWire.TurnOn();
        }
    }

    /*
     * Changes the materials in each of the buttonWires to their "off" versions.
     * Called in ?????.
     */
    public void DeactivateButton()
    {
        foreach (ButtonWire buttonWire in buttonWires)
        {
            buttonWire.TurnOff();
        }
    }
}

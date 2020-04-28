using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public ButtonWire[] buttonWires;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            ActivateButton();

        else if (Input.GetKeyDown(KeyCode.P))
            DeactivateButton();
    }

    public void ActivateButton()
    {
        foreach (ButtonWire buttonWire in buttonWires)
        {
            buttonWire.TurnOn();
        }
    }

    public void DeactivateButton()
    {
        foreach (ButtonWire buttonWire in buttonWires)
        {
            buttonWire.TurnOff();
        }
    }
}

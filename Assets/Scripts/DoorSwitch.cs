using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{

    public Animator doorControl;
    ButtonPresser ButtonPresser;
    public ButtonController ButtonController;

    public void DoorOpener()
    {
        ButtonController.ActivateButton();
        doorControl.SetTrigger("Open");
        Debug.Log("Fwooosh");


    }


    public void DoorClosed()
    {
        ButtonController.DeactivateButton();
        doorControl.SetTrigger("Close");
        Debug.Log("Reverse Foowsh");

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

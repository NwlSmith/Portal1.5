using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{

    public Animator doorControl;
    ButtonPresser ButtonPresser;
    public ButtonController ButtonController;

    private AudioSource AS;

    public void DoorOpener()
    {
        ButtonController.ActivateButton();
        doorControl.SetTrigger("Open");
        AS.Play();
        Debug.Log("Fwooosh");


    }


    public void DoorClosed()
    {
        ButtonController.DeactivateButton();
        doorControl.SetTrigger("Close");
        AS.Play();
        Debug.Log("Reverse Foowsh");

    }


    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

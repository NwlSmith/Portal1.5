using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPresser : MonoBehaviour
{
    //public Animation ButtonPressed;
    public Animator buttonPressed;

    public bool playerOnButton = false;

    public void DoorOpener()
    {




    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "CanPickUp")
        {
            buttonPressed.SetTrigger("ButtonPressed");

            Debug.Log("Button is pressed");
        }

       

    }


    private void OnCollisionExit()
    {

        buttonPressed.SetTrigger("ButtonReleased");

    }
}

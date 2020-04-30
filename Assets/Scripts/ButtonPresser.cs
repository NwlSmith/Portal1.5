using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPresser : MonoBehaviour
{
    /*
     * Date Created: 4/282/20
     * Creator: Raymond Lothian
     * 
     * Description: Checks to see if an object is on top of a button and plays corresponding animation.
     */

    //public Animation ButtonPressed;
    
    //
    public Animator buttonPressed;

    public bool playerOnButton = false;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks if a the mesh collider is being touched by anything
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "CanPickUp")
        {
            buttonPressed.SetTrigger("ButtonPressed");

            Debug.Log("Button is pressed");
        }

       

    }

    //Checks to see if anything has been moved from the mesh collider
    private void OnCollisionExit()
    {

        buttonPressed.SetTrigger("ButtonReleased");

    }
}

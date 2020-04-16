using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this class makes the portal objects move.  It also deals with what is and isn't portable.
public class PortalMove : MonoBehaviour
{

    public static bool connectedPortal;
    private float forceVar = 500;

    Rigidbody RB;

    //  AudioSource AS;
    // public AudioClip ball;
    // public AudioClip strike;



    // Start is called before the first frame update
    void Start()
    {
        // AS = GetComponent<AudioSource>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
 

    private void FixedUpdate()
    {
        RB.AddForce(transform.forward * forceVar);
    }

    private void OnCollisionEnter(Collision other)
    {
        //detect if its portable or not. change the string to whatever the tag is for objects.
        
        if (other.gameObject.name == "wallNo")
        {
            Destroy(gameObject);
        }
    //this one can be portaled.
        if (other.gameObject.name == "wall")
        {
            
            RB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}

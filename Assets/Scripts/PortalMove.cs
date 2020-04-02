using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMove : MonoBehaviour
{
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

   
}

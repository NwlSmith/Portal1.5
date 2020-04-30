using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/2/2020
 * Creator: Isabella Chen
 * 
 * Description: Put this script on the player object, then it should be able to pick up items.
 */
public class PickupObject : MonoBehaviour
{
    public GameObject mainCamera;
    public bool carrying; //check if player is carrying object
    public GameObject carriedObject;
    public float distance = 3;
    public float smooth = 4;
    private AudioSource AS;
    public AudioClip pickupClip;
    public AudioClip dropClip;


    void Start()
    {
        AS = GetComponent<AudioSource>();
        
        mainCamera = GameObject.FindWithTag("MainCamera"); 
        //Find the main camera with "MainCamera" Tag
    }

    void Update()
    {
        if (carrying)
        {
            checkDrop();
            //carry(carriedObject);
        }
        else
        {
            //play pickup sound
            
            Pickup();
            //if player is not carrying object, then it's able to pick up items
        }
    }

    void FixedUpdate()
    {
        if(carrying)
        {
            carry(carriedObject);
        }
    }
    public void carry(GameObject o)
    {
        // Retrieve the rigidbody
        Rigidbody rb = o.GetComponent<Rigidbody>();
        // Calculate the direction and magnitude of the difference in position from the object to the target location in front of the camera.
        Vector3 forceDir = o.transform.position - (mainCamera.transform.position + mainCamera.transform.forward * distance);
        // Set velocity to zero so we aren't orbiting the target.
        rb.velocity = rb.velocity * .5f;
        rb.angularVelocity = Vector3.zero;
        // Add force in that direction.
        rb.AddForce(- forceDir * 100000 * carriedObject.GetComponent<Rigidbody>().mass * Time.fixedDeltaTime);
        // Make the objects rotate like in the original portal.
        rb.MoveRotation(Quaternion.LookRotation(Camera.main.transform.forward));
    }
    public void Pickup()
    {
        if(Input.GetKeyDown(KeyCode.E)) //Press "E" key to pick up and place items
        {
            
            int x = Screen.width / 2;
            int y = Screen.height / 2; 
            //Find the middle point of screen and raycast from there

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y)); //assign the origin point to cast a ray
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 3)) //Casts a ray(Vector3 origin, Vector3 direction, float maxDistance), against all colliders in the Scene
            {
                //make it able to pick up all the objects with "Pickupable" script
                if(hit.collider.gameObject.CompareTag("CanPickUp"))
                {
                    //play pickup sound
                    AS.clip = pickupClip;
                    AS.Play();
                    carrying = true;
                    carriedObject = hit.collider.gameObject;
                }
            }
        }
    }

    public void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AS.clip = dropClip;
            AS.Play();
            dropObject();
        }
    }

    public void dropObject()
    {
        carrying = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }
}

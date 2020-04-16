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
  
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera"); 
        //Find the main camera with "MainCamera" Tag
    }

    void Update()
    {
        if(carrying)
        {
            carry(carriedObject);
            checkDrop();
        }
        else
        {
            Pickup(); 
            //if player is not carrying object, then it's able to pick up items
        }
    }
    public void carry(GameObject o)
    {
        
        o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth); //"Lerp" can smooth the movement of picked items
        o.transform.rotation = Quaternion.identity;
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
                Pickupable p = hit.collider.GetComponent<Pickupable>(); 
                //make it able to pick up all the objects with "Pickupable" script
                if(p != null)
                {
                    carrying = true;
                    carriedObject = p.gameObject;
                    p.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }

    public void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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

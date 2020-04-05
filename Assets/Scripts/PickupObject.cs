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
    GameObject mainCamera;
    bool carrying;
    GameObject carriedObject;
    public float distance = 3;
    public float smooth = 2;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
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
        }
    }
    void carry(GameObject o)
    {
        
        o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        o.transform.rotation = Quaternion.identity;
    }
    void Pickup()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
                if(p != null)
                {
                    carrying = true;
                    carriedObject = p.gameObject;
                    //p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    p.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }

    void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        carrying = false;
        //carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        p.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/21/2020
 * Creator: Nate Smith
 * 
 * Description: Handles rotation and destruction of the security camera.
 */
public class SecurityCamera : MonoBehaviour
{

    // Public Variables.
    public Transform camHolder;
    public Transform camObject;
    public bool alive = true;

    // Private Variables.
    private Transform playerCam;

    private void Start()
    {
        // Retrieve player camera transform.
        playerCam = FindObjectOfType<PlayerLook>().transform;
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            // Rotate the Camera to follow the player horizontally AND vertically.
            Vector3 target = playerCam.position - camObject.position;
            camObject.rotation = Quaternion.LookRotation(target);
            // Correct for model glitch
            camObject.Rotate(transform.up, 90);
            // Rotate the cameraHolder to follow the player vertically.
            target.y = 0f;
            camHolder.rotation = Quaternion.LookRotation(target);
            // Correct for model glitch
            camHolder.Rotate(transform.up, 90);

            Portal orange = PortalManager.instance.orange;
            Portal blue = PortalManager.instance.blue;
            if (orange && Vector3.Distance(orange.transform.position, transform.position) <= 2f)
                Detach();
            if (blue && Vector3.Distance(blue.transform.position, transform.position) <= 2f)
                Detach();

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision  with " + collision.gameObject.name);
        if (collision.gameObject.layer == 1 || collision.gameObject.layer == 11)
        {
            alive = false;
            gameObject.AddComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by " + other.gameObject.name);
        if (other.gameObject.layer == 1 || other.gameObject.layer == 11)
        {
            alive = false;
            gameObject.AddComponent<Rigidbody>();
        }
    }

    private void Detach()
    {
        alive = false;
        gameObject.AddComponent<Rigidbody>().mass = 10;
        tag = "CanPickUp";
        gameObject.AddComponent<ObjectUtility>();
        Destroy(this);
    }
}

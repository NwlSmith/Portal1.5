using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/31/2020
 * Creator: Nate Smith
 * 
 * Description: Moves the camera on the horizontal and vertical directions based on mouse movement.
 */
public class PlayerLook : MonoBehaviour
{
    // Public Variables.
    public float mouseSensitivity = 100f;

    // Private Variables.
    private Transform playerBody;
    private float xRot = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerBody = GetComponentInParent<CharacterController>().transform;
        if (playerBody == null)
        {
            Debug.Log("Parent of " + name + " does not contain a Character Controller.");
        }


    }

    void Update()
    {
        // Retrieve mouse input.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Move camera vertically.
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        // Rotate player horizontally.
        playerBody.Rotate(Vector3.up * mouseX);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Portal>(out Portal portal))
            Debug.Log("Camera collided with portal trigger");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Portal portal))
            Debug.Log("Camera collided with portal collider");
    }*/
}

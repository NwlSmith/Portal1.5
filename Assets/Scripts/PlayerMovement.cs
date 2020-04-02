using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/31/2020
 * Creator: Nate Smith
 * 
 * Description: Moves the player in the horizontal plane and handles gravity and jumping.
 */
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotSpeed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public Transform groundPos;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool onGround = false;
    public Vector3 physicsVelocity;

    private CharacterController charController;
    private float xInput = 0f;
    private float yInput = 0f;
    private float zInput = 0f;

    void Start()
    {
        if (!TryGetComponent(out charController))
        {
            Debug.Log(name + " does not contain a Character Controller.");
        }
    }

    void Update()
    {
        // Retrieve directional input.
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        if (onGround && Input.GetButtonDown("Jump") )
            yInput = 1f;
        else
            yInput = 0f;

        // Ensure the player is always upright.
        Quaternion upright = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, upright, moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Calculate movement vector.
        Vector3 move = transform.right * xInput + transform.forward * zInput;

        // Calculate physics movement.
        onGround = Physics.CheckSphere(groundPos.position, groundDistance, groundMask);
        //onGround = charController.isGrounded;
        if (onGround)
        {
            if (physicsVelocity.y < 0f)
                physicsVelocity.y = -2f;
            if (yInput == 1)
                physicsVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        physicsVelocity.y += gravity * Time.fixedDeltaTime;

        // Move player
        charController.Move((move * moveSpeed + physicsVelocity) * Time.fixedDeltaTime);
    }

    /*
     * Teleports the player to the other portal position.
     * Called in TeleportPlayer() in Portal.cs.
     */
    public void TeleportPlayer(Transform target)
    {
        charController.Move(target.position - transform.position);
        transform.rotation = target.rotation;
    }

    /*
     * Checks if the player is entering the portal.
     * If the velocity of the player dotted with the normal vector of the portal is less than 0,
     * meaning they are roughly opposite, return true.
     * Called in OnTriggerEnter() in Portal.cs.
     */
    public bool VelocityCheck(Vector3 targetNormal)
    {
        Debug.Log("PlayerVel " + charController.velocity + " targetNormal " + targetNormal + " dotted = " + Vector3.Dot(charController.velocity, targetNormal));
        if (Vector3.Dot(charController.velocity, targetNormal) < 0f)
            return true;
        return false;
    }
}

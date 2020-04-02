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
        charController = GetComponent<CharacterController>();
        if (charController == null)
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
}

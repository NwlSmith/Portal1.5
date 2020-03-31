using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 3/31/2020
 * Creator: Nate Smith
 * 
 * Description: Moves the player
 */
public class PlayerMovement : MonoBehaviour
{
    public float speed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public Transform groundPos;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool onGround = false;

    private CharacterController charController;
    private float xInput = 0f;
    private float yInput = 0f;
    private float zInput = 0f;
    private float realJumpHeight;
    private Vector3 velocity;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        if (charController == null)
        {
            Debug.Log(name + " does not contain a Character Controller.");
        }
        realJumpHeight = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

        
    }

    private void FixedUpdate()
    {
        // Calculate movement vector.
        Vector3 move = transform.right * xInput + transform.forward * zInput;

        // Move character horizontally.
        charController.Move(move * speed * Time.fixedDeltaTime);

        // Calculate vertical movement.
        onGround = Physics.CheckSphere(groundPos.position, groundDistance, groundMask);
        if (onGround)
        {
            if (velocity.y < 0f)
                velocity.y = -2f;
            if (yInput == 1)
                velocity.y = realJumpHeight;
        }
        velocity.y += gravity * Time.fixedDeltaTime;
        charController.Move(velocity * Time.fixedDeltaTime);
    }
}

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
    public float inAirMoveMultiplier = .2f;
    public float maxVelocity = 100f;
    public float maxRadius = .5f;
    [HideInInspector] public bool onGround = false;
    [HideInInspector] public Vector3 physicsVector;

    private CharacterController charController;
    private PlayerLook playerLook;
    private float xInput = 0f;
    private float yInput = 0f;
    private float zInput = 0f;

    void Start()
    {
        if (!TryGetComponent(out charController))
        {
            Debug.Log(name + " does not contain a Character Controller.");
        }
        if (GetComponentInChildren<PlayerLook>() == null)
        {
            Debug.Log(name + " does not contain a PlayerLook script.");
        }
        else
        {
            playerLook = GetComponentInChildren<PlayerLook>();
        }
    }

    void Update()
    {
        // Retrieve directional input.
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        yInput = Input.GetAxis("Jump");
        // If the player is in the air, they cannot jump and they shouldn't be able to move super quickly.
        if (!onGround)
        {
            xInput *= inAirMoveMultiplier;
            zInput *= inAirMoveMultiplier;
            yInput = 0;
        }

        // DEBUG: Press escape to pause the editor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Break();
            Application.Quit();
        }

        // Ensure the player is always upright.
        Quaternion upright = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, upright, rotSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Calculate input movement vector.
        Vector3 moveVector = transform.right * xInput + transform.forward * zInput;

        // Calculate physics movement.
        if (onGround && !charController.isGrounded)
        {
            physicsVector += moveVector * moveSpeed;
        }
        onGround = charController.isGrounded;
        if (onGround)
        {
            // When on the ground, the player shouldn't have any horizontal velocity other than input movement.
            physicsVector.x = 0f;
            physicsVector.z = 0f;
            // When on the ground, the player's vertical velocity doesn't need to increase with gravity.
            if (physicsVector.y < 0f)
                physicsVector.y = -2f;
            if (yInput >= 0.1f)
            {
                // Save the player's input movement so it will continue with same velocity while in air.
                physicsVector += moveVector * moveSpeed;
                // Jump.
                physicsVector.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                onGround = false;
            }
        }
        // Increment physics gravity.
        physicsVector.y += gravity * Time.fixedDeltaTime;

        // Move player according to its input and physics
        charController.Move((moveVector * moveSpeed + physicsVector) * Time.fixedDeltaTime);

        // Resize the collider sphere which is used for calculating when to teleport.
        // Going faster means the sphere will be larger, thus allowing for more collision detection.
        playerLook.GetComponent<SphereCollider>().radius =
            Mathf.Lerp(.1f, maxRadius, Vector3.ClampMagnitude(charController.velocity, maxVelocity).magnitude / maxVelocity);
    }

    /*
     * Teleports the player to the other portal position.
     * The angle of the back of the origin teleporter relative to the player added to the angle of the target portal.
     * Called in TeleportPlayer() in Portal.cs.
     */
    public void TeleportPlayer(Transform originPortal, Transform targetPortal)
    {
        // Temporarily disable the CharacterController to allow teleportation.
        charController.enabled = false;
        transform.position = targetPortal.position;
        charController.enabled = true;

        // Set the players look direction to the same direction you entered in relation to the new portal.
        Vector3 dirTransformVector = targetPortal.rotation.eulerAngles - originPortal.rotation.eulerAngles + new Vector3(0, 180, 0) + transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(dirTransformVector);

        // Transfer velocity to new direction.
        physicsVector = targetPortal.forward * physicsVector.magnitude;
    }

    /*
     * Checks if the player is entering the portal.
     * If the velocity of the player dotted with the normal vector of the portal is less than 0,
     * meaning they are roughly opposite, return true.
     * Called in OnTriggerEnter() in Portal.cs.
     */
    public bool VelocityCheck(Vector3 targetNormal)
    {
        if (Vector3.Dot(charController.velocity.normalized, targetNormal) < 0f)
            return true;
        return false;
    }
}

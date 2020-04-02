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
    public Vector3 physicsVector;

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
        
        if (onGround)
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            yInput = Input.GetAxis("Jump");
        }
        else
            yInput = 0f;

        // Ensure the player is always upright.
        Quaternion upright = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, upright, moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Calculate movement vector.
        Vector3 moveVector = transform.right * xInput + transform.forward * zInput;

        // Calculate physics movement.
        //onGround = Physics.CheckSphere(groundPos.position, groundDistance, groundMask);
        onGround = charController.isGrounded;
        //onGround = charController.isGrounded;
        if (onGround)
        {
            physicsVector.x = 0f;
            physicsVector.z = 0f;
            if (physicsVector.y < 0f)
                physicsVector.y = -2f;
            if (yInput >= 0.1f)
                physicsVector.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        physicsVector.y += gravity * Time.fixedDeltaTime;

        // Move player
        charController.Move((moveVector * moveSpeed + physicsVector) * Time.fixedDeltaTime);
    }

    /*
     * Teleports the player to the other portal position.
     * The angle of the back of the origin teleporter relative to the player added to the angle of the target portal.
     * Called in TeleportPlayer() in Portal.cs.
     */
    public void TeleportPlayer(Transform originPortal, Transform targetPortal)
    {
        // Vector3.Reflect()?
        charController.Move(targetPortal.position - transform.position);
        Vector3 dirTransformVector = targetPortal.rotation.eulerAngles + (originPortal.rotation.eulerAngles + new Vector3(0, 180, 0) - transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Euler(dirTransformVector);
        physicsVector = targetPortal.forward.normalized * physicsVector.magnitude;
        Debug.Log("physicsVector " + physicsVector);
    }

    private Vector3 TransformedVector(Vector3 originRot, Vector3 targetRot)
    {
        return Vector3.zero;
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
        if (Vector3.Dot(charController.velocity.normalized, targetNormal) < 0f)
            return true;
        return false;
    }
}

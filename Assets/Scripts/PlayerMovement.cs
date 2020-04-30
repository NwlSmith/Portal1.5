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
    public Vector3 physicsVector;

    private CharacterController charController;
    private PlayerLook playerLook;
    private FollowRotation followRotation;
    private float xInput = 0f;
    private float yInput = 0f;
    private float zInput = 0f;

<<<<<<< HEAD
    public GameObject button;

=======

    public AudioSource AS;

    public AudioClip throughPortalClip;
>>>>>>> 78e0aa3b964121e14152492fcd63e5f98cdb5162
    void Start()
    {
        AS = GetComponent<AudioSource>();
        
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
        if (GetComponentInChildren<FollowRotation>() == null)
        {
            Debug.Log(name + " does not contain a FollowRotation script.");
        }
        else
        {
            followRotation = GetComponentInChildren<FollowRotation>();
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

        // Ensure the player is always upright.
        Quaternion upright = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, upright, rotSpeed * Time.deltaTime);


        Ray toeRay = new Ray(transform.position, Vector3.down * 1.4f);
        Debug.DrawRay(toeRay.origin, toeRay.direction * 1.4f, Color.blue);

        if (Physics.Raycast(toeRay.origin, toeRay.direction, out RaycastHit toeHit, 1.4f))

        {
            //GameObject button;
            if (toeHit.transform.gameObject.tag == "Button" && button == null &&
               toeHit.transform.gameObject.GetComponent<ButtonPresser>().playerOnButton == false)
                {
                    button = toeHit.transform.gameObject; // assign button that we just hit with toes
                    toeHit.transform.gameObject.GetComponent<ButtonPresser>().playerOnButton = true;
                button.GetComponent<ButtonPresser>().buttonPressed.SetTrigger("ButtonPressed");


                     Debug.Log("You hit"+ toeHit.transform.gameObject);
                 }
            else if (toeHit.transform.gameObject.tag != "Button" && button !=null)
            {
                button.GetComponent<ButtonPresser>().playerOnButton = false;
                Debug.Log("You stepped off the button");
                button.GetComponent<ButtonPresser>().buttonPressed.SetTrigger("ButtonReleased");
                button = null;

            }
        }

        // if your midair and already assigned a button, raycast shouldn't be hitting anything
        if (!Physics.Raycast(toeRay.origin, toeRay.direction, out RaycastHit noHit, 1.4f) && button != null)
        {
            button.GetComponent<ButtonPresser>().playerOnButton = false;
            Debug.Log("You stepped off the button");
            button.GetComponent<ButtonPresser>().buttonPressed.SetTrigger("ButtonReleased");
            button = null;
        }


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

        // Clamp velocity.
        physicsVector.y = Mathf.Clamp(physicsVector.y, -100f, 100f);

        // Move player according to its input and physics
        charController.Move((moveVector * moveSpeed + physicsVector) * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       


        Vector3 collisionDirection = hit.normal;
        if (collisionDirection == Vector3.down)
        {
            if (GameManager.instance.debug)
                Debug.Log("Ooop! ya hit yer head there didn't ya?");
            physicsVector.y = 0f;
        }
        
        if (!charController.isGrounded)
        {
            if (GameManager.instance.debug)
                Debug.Log("Ooop! yer not on the ground and ya hit sumthin!");

            
            if (collisionDirection == Vector3.right || collisionDirection == Vector3.left)
            {
                if (GameManager.instance.debug)
                    Debug.Log("Ooop! ya hit the wall in the x axis there didn't ya?");
                physicsVector.x = 0f;
            }
            else if (collisionDirection == Vector3.forward || collisionDirection == Vector3.back)
            {
                if (GameManager.instance.debug)
                    Debug.Log("Ooop! ya hit the wall in the z axis there didn't ya?");
                physicsVector.z = 0f;
            }
        }
    }

    /*
     * Teleports the player to the other portal position.
     * The angle of the back of the origin teleporter relative to the player added to the angle of the target portal.
     * Called in TeleportPlayer() in Portal.cs.
     */
    public void TeleportPlayer(Transform originPortal, Transform targetPortal)
    {
        //play portal through sound
        AS.clip = throughPortalClip;
        AS.Play();
        
        // Temporarily disable the CharacterController to allow teleportation.
        charController.enabled = false;
        transform.position = targetPortal.TransformPoint(Quaternion.Euler(0f, 180f, 0f) * originPortal.InverseTransformPoint(transform.position));
        charController.enabled = true;

        // Set the players look direction to the same direction you entered in relation to the new portal.
        Vector3 dirTransformVector = targetPortal.rotation.eulerAngles - originPortal.rotation.eulerAngles + new Vector3(0, 180, 0) + transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(dirTransformVector);

        // Transfer velocity to new direction.
        physicsVector = targetPortal.forward * physicsVector.magnitude;

        followRotation.Teleport();
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

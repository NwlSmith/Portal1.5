using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/8/2020
 * Creator: Nate Smith
 * 
 * Description: Extension Functions for Rigidbodies.
 */
public static class RigidbodyExt
{

    /*
     * Checks if the given object is entering the portal.
     * If the velocity of the object dotted with the normal vector of the portal is less than 0,
     * meaning they are roughly opposite, return true.
     * Called in OnTriggerEnter() in Portal.cs.
     */
    public static bool VelocityCheck(this Rigidbody rb, Vector3 portalForward)
    {
        if (Vector3.Dot(rb.velocity.normalized, portalForward) < 0f)
            return true;
        return false;
    }

    /*
     * Teleports the object to the other portal.
     * Called in OnTriggerEnter() in Portal.cs.
     */
    public static void TeleportObject(this Rigidbody rb, Transform originPortal, Transform targetPortal)
    {
        Vector3 vel = rb.velocity;
        // Move the object to the target portal.
        rb.transform.position = targetPortal.TransformPoint(Quaternion.Euler(0f, 180f, 0f) * originPortal.InverseTransformPoint(rb.position));

        // Set the objects rotation direction to the same direction it entered in relation to the new portal.
        Quaternion relativeRotation = Quaternion.Inverse(originPortal.rotation) * rb.transform.rotation;
        relativeRotation = Quaternion.Euler(0f, 180f, 0f) * relativeRotation;
        rb.transform.rotation = targetPortal.rotation * relativeRotation;

        rb.isKinematic = false;

        bool wasMovingDown = rb.velocity.y < 0f && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x) && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.z);

        Debug.Log("velocity = " + rb.velocity + " vel = "+vel);

        if (rb.velocity.magnitude < 2f)
        {
            Debug.Log("vel < 2");
            rb.velocity = targetPortal.forward.normalized * (rb.velocity.magnitude + 0.1f);
        }
        else
        {
            Debug.Log("vel NOT < 2");
            rb.velocity = targetPortal.forward.normalized * (rb.velocity.magnitude + 0.1f);
        }

        bool willMoveUp = rb.velocity.y > 0f && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x) && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.z);
        if (GameManager.instance.debug)
        {
            Debug.Log("rb.velocity.y " + rb.velocity.y + " x " + rb.velocity.x + " z " + rb.velocity.z);
            Debug.Log("rb.velocity.y > 0f " + (rb.velocity.y > 0f) + " Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x) " + (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x)) + " && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.z);" + (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.z)));
            Debug.Log("was moving down? " + wasMovingDown);
            Debug.Log("will move up? " + willMoveUp);
        }
        // Transfer velocity to new direction.
        // If the object is going to exit the portal moving upwards, give it a minimum velocity.
        if (wasMovingDown || willMoveUp)
        {
            Debug.Log("vel was moving down / will move up");
            rb.velocity = targetPortal.forward.normalized * Mathf.Max(rb.velocity.magnitude, 4f);
        }
        
    }
}
﻿using System.Collections;
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
        Debug.Log("Functionality unfinished");
        // Temporarily disable the CharacterController to allow teleportation.
        rb.position = targetPortal.position;

        // Set the players look direction to the same direction you entered in relation to the new portal.
        Vector3 dirTransformVector = targetPortal.rotation.eulerAngles - originPortal.rotation.eulerAngles + new Vector3(0, 180, 0) + targetPortal.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(dirTransformVector);

        // Transfer velocity to new direction.
        rb.velocity = targetPortal.forward.normalized * Mathf.Max(rb.velocity.magnitude, 10f);
    }


}
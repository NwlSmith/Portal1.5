using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/13/2020
 * Creator: Nate Smith
 * 
 * Description: Utility function for pickupable portalable objects.
 */
public class ObjectUtility : MonoBehaviour
{
    // Public Variables.
    public int layer;
    public Portal enteredPortal;

    // Private Variables.
    private Quaternion rotationAdjustment = Quaternion.Euler(0, 180, 0);
    private GameObject clone;

    private void Awake()
    {
        // Create a clone of this object with only its mesh.
        clone = new GameObject();
        MeshFilter mf = clone.AddComponent<MeshFilter>();
        MeshRenderer mr = clone.AddComponent<MeshRenderer>();
        mf.mesh = GetComponent<MeshFilter>().mesh;
        mr.materials = GetComponent<MeshRenderer>().materials;
        clone.transform.localScale = transform.localScale;
        clone.name = name + " Clone";
        clone.SetActive(false);

        // Set tag.
        tag = "CanPickUp";

        // Create a child gameobject to detect when the center of the collider enters the portal.
        GameObject centralColliderChild = new GameObject();
        BoxCollider bc = centralColliderChild.AddComponent<BoxCollider>();
        bc.size = Vector3.zero;
        centralColliderChild.transform.parent = gameObject.transform;
        centralColliderChild.transform.localPosition = Vector3.zero;
        centralColliderChild.tag = "CanPickUp";
        centralColliderChild.layer = 10;
        centralColliderChild.name = "Central Collider";
    }

    private void Update()
    {
        // If there are not two portals, ignore this.
        if (PortalManager.instance.blue == null || PortalManager.instance.orange == null)
            return;

        // If there are two portals and the object has entered PortalWallDisable...
        if (enteredPortal != null && PortalManager.instance.blue != null && PortalManager.instance.orange != null)
        {
            ReflectClone();
        }
        else
        {
            clone.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public void ReflectClone()
    {
        // Activate the clone
        clone.SetActive(true);

        // Reflect its rotation on the opposite portal
        Quaternion relativeRotation = Quaternion.Inverse(enteredPortal.transform.rotation) * transform.rotation;
        relativeRotation = rotationAdjustment * relativeRotation;
        clone.transform.rotation = PortalManager.instance.OtherPortal(enteredPortal).transform.rotation * relativeRotation;

        // Reflect its position on the opposite portal
        Vector3 relativePosition = enteredPortal.transform.InverseTransformPoint(transform.position);
        relativePosition = rotationAdjustment * relativePosition;
        clone.transform.position = PortalManager.instance.OtherPortal(enteredPortal).transform.TransformPoint(relativePosition);
    }
}

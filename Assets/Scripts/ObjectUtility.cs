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
        centralColliderChild.tag = "CanPickUp";
        centralColliderChild.layer = 10;
        centralColliderChild.name = "Central Collider";
    }

    private void LateUpdate()
    {
        if (enteredPortal != null && PortalManager.instance.blue != null && PortalManager.instance.orange != null)
        {
            clone.SetActive(true);
            Vector3 relativePosition = enteredPortal.transform.InverseTransformPoint(transform.position);
            relativePosition = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePosition;
            clone.transform.position = PortalManager.instance.OtherPortal(enteredPortal).transform.TransformPoint(relativePosition);
        }
        else
        {
            clone.SetActive(false);
        }
    }
}

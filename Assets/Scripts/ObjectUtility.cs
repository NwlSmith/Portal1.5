using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/13/2020
 * Creator: Nate Smith
 * 
 * Description: Utility function for pickupable portalable objects.
 * Creates a point collider at the center of the object which triggers the teleport function.
 * Creates a clone "ghost" mesh that reflects the object across portals to prevent object clipping
 */
public class ObjectUtility : MonoBehaviour
{
    // Public Variables.
    public int layer;
    [HideInInspector] public Portal enteredPortal;

    // Private Variables.
    private Quaternion rotationAdjustment = Quaternion.Euler(0, 180, 0);
    private GameObject clone;
    private AudioSource audioSource;
    private Rigidbody rb;


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

        // Retrieve the audio source.
        if (!TryGetComponent(out audioSource))
        {
            Debug.Log("ERROR: object " + name + " created without AudioSource.");
        }
        
        // Retrieve the rigidbody.
        if (!TryGetComponent(out rb))
        {
            Debug.Log("ERROR: object " + name + " created without Rigidbody.");
        }
    }

   /*
    * Either reflect the clone or do not.
    */
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

    /*
    * Play collision sound if object hits another with enough speed.
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude >= .5f)
        {
            audioSource.pitch = Random.Range(.95f, 1.05f);
            audioSource.Play();
        }
    }

    /*
    * Reflects the clone object's in terms of this objects rotation and position in relation to the entered portal.
    * Called in Update().
    */
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

    /*
    * Destroys object.
    * Plays animation, sound, and destroys gameobject after delay.
    * Called in ________________ in __________.cs.
    */
    public void DestroyMe()
    {
        // If the player is carrying the object, drop it.
        PickupObject po = FindObjectOfType<PickupObject>();
        if (po.carriedObject == gameObject)
            po.dropObject();

        // Destroy object after delay.
        Destroy(gameObject, .5f);
    }
}

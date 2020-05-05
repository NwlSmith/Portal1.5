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

    public AudioClip dropSound;
    public AudioClip destroySound;

    // Private Variables.
    private Quaternion rotationAdjustment = Quaternion.Euler(0, 180, 0);
    private GameObject clone;
    private AudioSource audioSource;
    private Rigidbody rb;


    private void Awake()
    {
        // Create a clone of this object with only its mesh.
        clone = new GameObject();
        if (GetComponent<MeshRenderer>())
        {
            MeshFilter mf = clone.AddComponent<MeshFilter>();
            MeshRenderer mr = clone.AddComponent<MeshRenderer>();
            mf.mesh = GetComponent<MeshFilter>().mesh;
            mr.materials = GetComponent<MeshRenderer>().materials;
        }
        foreach (Transform childTr in transform)
        {
            CloneChild(clone, childTr);
        }
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

    private void CloneChild(GameObject parent, Transform cloneChildTrans)
    {
        GameObject cloneChildGO = new GameObject();
        cloneChildGO.transform.parent = parent.transform;

        cloneChildGO.transform.localPosition = cloneChildTrans.transform.localPosition;
        cloneChildGO.transform.localRotation = cloneChildTrans.transform.localRotation;
        cloneChildGO.transform.localScale = cloneChildTrans.transform.localScale;

        if (cloneChildTrans.TryGetComponent(out MeshFilter mf))
        {
            mf = cloneChildGO.AddComponent<MeshFilter>();
            MeshRenderer mr = cloneChildGO.AddComponent<MeshRenderer>();
            mf.mesh = cloneChildTrans.GetComponent<MeshFilter>().mesh;
            mr.materials = cloneChildTrans.GetComponent<MeshRenderer>().materials;
        }

        foreach (Transform childTr in cloneChildTrans)
        {
            CloneChild(cloneChildGO, childTr);
        }
    }

    /*
     * Either reflect the clone or do not.
     */
    private void Update()
    {

        Debug.DrawRay(transform.position, rb.velocity.normalized, Color.cyan);
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
     * Teleports the object to the other portal.
     * Called in OnTriggerEnter() in Portal.cs.
     */
    public void TeleportObject(Transform originPortal, Transform targetPortal)
    {
        float vel = rb.velocity.magnitude;
        // Move the object to the target portal.
        rb.transform.position = targetPortal.TransformPoint(Quaternion.Euler(0f, 180f, 0f) * originPortal.InverseTransformPoint(rb.position));

        // Set the objects rotation direction to the same direction it entered in relation to the new portal.
        Quaternion relativeRotation = Quaternion.Inverse(originPortal.rotation) * rb.transform.rotation;
        relativeRotation = Quaternion.Euler(0f, 180f, 0f) * relativeRotation;
        rb.transform.rotation = targetPortal.rotation * relativeRotation;

        rb.isKinematic = false;
        rb.velocity = targetPortal.forward.normalized * Mathf.Max(rb.velocity.magnitude, 4f);
        /*
        bool wasMovingDown = rb.velocity.y < 0f && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x) && Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.z);

        Debug.Log("velocity = " + rb.velocity + " vel = " + vel);

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
        */
    }

    /*
    * Play collision sound if object hits another with enough speed.
    */
    private void OnCollisionEnter(Collision collision)
    {
        int otherLayer = collision.gameObject.layer;
        bool shouldCollide = true; // (layer == 12 && (otherLayer == 15 || otherLayer == 17)) || (layer == 13 && (otherLayer == 16 || otherLayer == 17)) || (layer == 14 && (otherLayer == 15 || otherLayer == 16 || otherLayer == 17));
        if (rb.velocity.magnitude >= .5f && shouldCollide)
        {
            audioSource.pitch = Random.Range(.9f, 1.1f);
            audioSource.clip = dropSound;
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
        clone.transform.rotation = enteredPortal.Other().transform.rotation * relativeRotation;

        // Reflect its position on the opposite portal
        Vector3 relativePosition = enteredPortal.transform.InverseTransformPoint(transform.position);
        relativePosition = rotationAdjustment * relativePosition;
        clone.transform.position = enteredPortal.Other().transform.TransformPoint(relativePosition);
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
        audioSource.clip = destroySound;
        audioSource.Play();
        Destroy(gameObject, .5f);
    }
}

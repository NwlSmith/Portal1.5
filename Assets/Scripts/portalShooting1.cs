using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/2/2020
 * Creator: Mark Timchenko
 * 
 * Description: Allows the player to shoot Blue Portals.
 */
public class portalShooting1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject portal;
    public GameObject portalRight;
    public float speed = 50;

    public float length = 1000f;
    public GameObject aimer;

    public LayerMask layerMask;

    private Camera cam;
    private bool portalDelay;



    void Start()
    {
        if (PortalManager.instance != null)
            portal = PortalManager.instance.bluePrefab;
        portalRight = PortalManager.instance.orangePrefab;
        // ball = GetComponent<GameObject>();
    }

    void Update()
    {

        // float mouseX = Input.GetAxis("Mouse X");
        // float mouseY = Input.GetAxis("Mouse Y");
        // transform.Rotate(0, mouseX * 5, 0);

        // Camera.main.transform.Rotate(-mouseY * 5, 0, 0);


        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(myRay.origin, myRay.direction * length, Color.red);
        RaycastHit myHit;

        //RaycastHit hit;
        // if(Input.GetMouseButtonUp(0)){
        //  Debug.Log("things");
        //  Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //  if(Physics.Raycast(myRay, out hit, 400.0f))
        //  {
        //     GameObject newBall = Instantiate(ball, transform.position, transform.rotation);
        //     newBall.GetComponent<Rigidbody>().velocity = (hit.point - transform.position).normalized * speed;
        //  }
        //  }
        Physics.Raycast(myRay, out myHit, length, layerMask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(myHit.point, myHit.normal, Color.blue);
        Debug.DrawRay(myHit.point, transform.rotation * Vector3.right, Color.red);
        Debug.DrawRay(myHit.point, Vector3.Cross(transform.rotation * Vector3.right, myHit.normal), Color.green);

        if (Physics.Raycast(myRay, out myHit, length, layerMask, QueryTriggerInteraction.Ignore))
        {
            // myHit.transform.Rotate(1,0,0);
            aimer.transform.position = myHit.point;
            //if (myHit.collider.gameObject.name != "wallNo")
            // {
            if (Input.GetMouseButtonDown(0) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                StartCoroutine(delayPortal());
                InstantiatePortal(myHit, portal);

            }

            if (Input.GetMouseButtonDown(1) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                StartCoroutine(delayPortal());
                InstantiatePortal(myHit, portalRight);

            }



            //  }


        }
    }

    /*
     * Instantiate a new portal at hit position, and at the proper rotation.
     */
    private void InstantiatePortal(RaycastHit hit, GameObject p)
    {
        // Instantiate a new portal.
        GameObject newPortal = Instantiate(p);
        // Set its parent to the root.
        newPortal.transform.SetParent(null);
        // The portal should face toward the direction perpendicular to the hit.


        // If the portal is on the floor (facing upward) or the ceiling (facing downward).
        if (hit.normal == Vector3.up || hit.normal == -Vector3.up)
        {
            Debug.Log("Portal placed on the floor or ceiling.");

            // The portal will face the direction perpendicular to the hit.
            Vector3 pForward = hit.normal;
            // The portal will be oriented so its bottom is closest to the player.
            Vector3 pRight = transform.rotation * Vector3.right;
            // The top of the portal will be opposite the bottom (kinda, it's complicated).
            Vector3 pUp = Vector3.Cross(pRight, pForward);

            // Set the portal to this new rotation.
            newPortal.transform.rotation = Quaternion.LookRotation(pForward, pUp);
        }
        // Otherwise, if the portal is on the wall/on a slant (facing anywhere else).
        else
        {
            Debug.Log("Portal placed on a wall.");newPortal.transform.forward = hit.normal;
        }

        // Set portal position
        newPortal.transform.position = hit.point + .01f * hit.normal;
        newPortal.GetComponent<Portal>().surface = hit.collider.gameObject;
    }

    IEnumerator delayPortal()
    {
        portalDelay = true;
        yield return new WaitForSeconds(.05f);
        portalDelay = false;
    }
}

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
    public LayerMask wallMask; // NEW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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

        // NEW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! \/
        Physics.Raycast(myRay, out myHit, length, layerMask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(myHit.point, myHit.normal, Color.blue);
        Debug.DrawRay(myHit.point, transform.rotation * Vector3.right, Color.red);
        Debug.DrawRay(myHit.point, Vector3.Cross(transform.rotation * Vector3.right, myHit.normal), Color.green);

        // NEW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ^

        if (Physics.Raycast(myRay, out myHit, length, layerMask, QueryTriggerInteraction.Ignore))
        {
            // myHit.transform.Rotate(1,0,0);
            aimer.transform.position = myHit.point;
            //if (myHit.collider.gameObject.name != "wallNo")
            // {
            if (Input.GetMouseButtonDown(0) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                StartCoroutine(delayPortal());
                InstantiatePortal(myHit, portal); // NEW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            }

            if (Input.GetMouseButtonDown(1) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                StartCoroutine(delayPortal());
                InstantiatePortal(myHit, portalRight); // NEW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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

        // Calculate the rotation of the new portal
        CalculateRotation(hit, newPortal);

        // Calculate the position of the new portal
        CalculatePosition(hit, newPortal);

        // Assign surface to this portal.
        newPortal.GetComponent<Portal>().surface = hit.collider.gameObject;
    }

    /*
    * Calculate the rotation of the new portal.
    */
    private void CalculateRotation(RaycastHit hit, GameObject newPortal)
    {
        // If the portal is on the floor (facing upward) or the ceiling (facing downward).
        if (hit.normal == Vector3.up || hit.normal == -Vector3.up)
        {
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
            Debug.Log("Portal placed on a wall.");

            // The portal should be upright, facing outward from its wall.
            newPortal.transform.forward = hit.normal;
        }
    }

    /*
     * Calculate the position of the new portal.
     */
    private void CalculatePosition(RaycastHit hit, GameObject newPortal)
    {
        // Set the initial position, which may change later, based off the position of the raycast hit, moved forward slightly.
        newPortal.transform.position = hit.point + .001f * hit.normal;


        // Fix Overhangs.

        // Use raycasts from each of the edges, going inward.
        // If the raycasts hit anything, that means the portal is not completely on the wall,
        // so move it over by the distance from the origin to the hit.
        // Origins of the detection rays relative to the parent.
        List<Vector3> testPtsOrigin = new List<Vector3> {
            newPortal.transform.TransformPoint( 0.0f,  2.1f, -0.1f), // Top
            newPortal.transform.TransformPoint( 1.1f,  0.0f, -0.1f), // Right
            newPortal.transform.TransformPoint( 0.0f, -2.1f, -0.1f), // Bottom
            newPortal.transform.TransformPoint(-1.1f,  0.0f, -0.1f)  // Left
        };
        // Directions of the detection rays.
        List<Vector3> testPtsDir = new List<Vector3> {
            newPortal.transform.TransformDirection(Vector3.down), // Top
            newPortal.transform.TransformDirection(Vector3.left), // Right
            newPortal.transform.TransformDirection(Vector3.up),   // Bottom
            newPortal.transform.TransformDirection(Vector3.right) // Left
        };

        // Go through each point.
        for (int i = 0; i < 4; i++)
        {
            // Check if the origin of the ray already is on the wall, if so, ignore this loop.
            if (Physics.CheckSphere(testPtsOrigin[i], 0.05f, wallMask))
                continue;
            // Otherwise, check the raycast and move it by the distance from the origin to the wall.
            else if (Physics.Raycast(testPtsOrigin[i], testPtsDir[i], out hit, (i % 2 == 0 ? 2.1f : 1.1f), wallMask))
                newPortal.transform.Translate(hit.point - testPtsOrigin[i], Space.World);
        }


        // Fix Intersections.
        // Use raycasts from the center to each of the edges, going outward.
        // If the raycasts hit anything, that means the portal is partly colliding with a wall,
        // so move it over by the distance distance to the edge, minus the distance from the origin to the hit.
        for (int i = 0; i < 4; i++)
        {
            // Raycast from slightly in front of the portal, outward in each direction.
            if (Physics.Raycast(newPortal.transform.TransformPoint(0.0f, 0.0f, 0.1f), testPtsDir[i], out hit, (i % 2 == 0 ? 2.1f : 1.1f), wallMask))
            {
                Vector3 posOffset = hit.point - newPortal.transform.TransformPoint(0.0f, 0.0f, 0.1f);
                newPortal.transform.Translate(-testPtsDir[i] * ((i % 2 == 0 ? 2.1f : 1.1f) - posOffset.magnitude), Space.World);
            }
        }
    }

    IEnumerator delayPortal()
    {
        portalDelay = true;
        yield return new WaitForSeconds(.05f);
        portalDelay = false;
    }
}

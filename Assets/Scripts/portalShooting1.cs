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

    public static bool shotOrange = false;
    public static bool shotBlue = false;

    public float length = 1000f;

    public LayerMask layerMask;
    public LayerMask wallMask; // NEW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    private Camera cam;
    private bool portalDelay;

    // The horizontal and vertical dimensions of the portal.
    private readonly float hor = 2.1f;
    private readonly float ver = 4.1f;



    void Start()
    {
        if (PortalManager.instance != null)
            portal = PortalManager.instance.bluePrefab;
        portalRight = PortalManager.instance.orangePrefab;
        // ball = GetComponent<GameObject>();
    }

    void Update()
    {

        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(myRay.origin, myRay.direction * length, Color.red);
        RaycastHit myHit;

        Physics.Raycast(myRay, out myHit, length, layerMask, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(myHit.point, myHit.normal, Color.blue);
        Debug.DrawRay(myHit.point, transform.rotation * Vector3.right, Color.red);
        Debug.DrawRay(myHit.point, Vector3.Cross(transform.rotation * Vector3.right, myHit.normal), Color.green);

        if (Physics.Raycast(myRay, out myHit, length, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (Input.GetMouseButtonDown(0) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                StartCoroutine(delayPortal());
                shotBlue = true;
                InstantiatePortal(myHit, portal);

            }

            if (Input.GetMouseButtonDown(1) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                StartCoroutine(delayPortal());
                shotOrange = true;
                InstantiatePortal(myHit, portalRight);

            }
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

        // Assign surface to this portal.
        newPortal.GetComponent<Portal>().surface = hit.collider.gameObject;

        // Calculate the rotation of the new portal
        CalculateRotation(hit, newPortal);

        // Calculate the position of the new portal
        CalculatePosition(hit, newPortal);
    }

    /*
    * Calculate the rotation of the new portal.
    */
    private void CalculateRotation(RaycastHit hit, GameObject newPortal)
    {
        // Get the new portal's transform.
        Transform newTrans = newPortal.transform;

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
            newTrans.rotation = Quaternion.LookRotation(pForward, pUp);
        }
        // Otherwise, if the portal is on the wall/on a slant (facing anywhere else).
        else
        {
            Debug.Log("Portal placed on a wall.");

            // The portal should be upright, facing outward from its wall.
            newTrans.forward = hit.normal;
        }
    }

    /*
     * Calculate the position of the new portal.
     */
    private void CalculatePosition(RaycastHit hit, GameObject newPortal)
    {
        // Get the new portal's transform.
        Transform newTrans = newPortal.transform;

        // Set the initial position, which may change later, based off the position of the raycast hit, moved forward slightly.
        newTrans.position = hit.point + .001f * hit.normal;

        bool moved = true;
        int iterations = 0;

        // Loop until the portal doesn't move or until it loops 3 times.
        while (moved && iterations < 3)
        {
            moved = false;
            iterations++;
            // Fix Overhangs.

            // Use raycasts from each of the edges, going inward.
            // If the raycasts hit anything, that means the portal is not completely on the wall,
            // so move it over by the distance from the origin to the hit.
            // Origins of the detection rays relative to the parent.
            List<Vector3> testPtsOrigin = new List<Vector3> {
                newTrans.TransformPoint( 0.0f,  2.1f, -0.1f), // Top
                newTrans.TransformPoint( 1.1f,  0.0f, -0.1f), // Right
                newTrans.TransformPoint( 0.0f, -2.1f, -0.1f), // Bottom
                newTrans.TransformPoint(-1.1f,  0.0f, -0.1f)  // Left
            };
            // Directions of the detection rays.
            List<Vector3> testPtsDir = new List<Vector3> {
                newTrans.TransformDirection(Vector3.down), // Top
                newTrans.TransformDirection(Vector3.left), // Right
                newTrans.TransformDirection(Vector3.up),   // Bottom
                newTrans.TransformDirection(Vector3.right) // Left
            };

            // Go through each point.
            for (int i = 0; i < 4; i++)
            {
                // Check if the origin of the ray already is on the wall, if so, ignore this loop.
                if (Physics.CheckSphere(testPtsOrigin[i], 0.05f, wallMask))
                    continue;
                // Otherwise, check the raycast and move it by the distance from the origin to the wall.
                else if (Physics.Raycast(testPtsOrigin[i], testPtsDir[i], out hit, (i % 2 == 0 ? 2.1f : 1.1f), wallMask))
                {
                    newTrans.Translate(hit.point - testPtsOrigin[i], Space.World);
                    moved = true;
                }
            }


            // Fix Intersections.

            // Use raycasts from the center to each of the edges, going outward.
            // If the raycasts hit anything, that means the portal is partly colliding with a wall,
            // so move it over by the distance distance to the edge, minus the distance from the origin to the hit.
            for (int i = 0; i < 4; i++)
            {
                // Raycast from slightly in front of the portal, outward in each direction.
                if (Physics.Raycast(newTrans.TransformPoint(0.0f, 0.0f, 0.1f), testPtsDir[i], out hit, (i % 2 == 0 ? 2.1f : 1.1f), wallMask))
                {
                    Vector3 posOffset = hit.point - newTrans.TransformPoint(0.0f, 0.0f, 0.1f);
                    newTrans.Translate(-testPtsDir[i] * ((i % 2 == 0 ? 2.1f : 1.1f) - posOffset.magnitude), Space.World);
                    moved = true;
                }
            }


            // Fix collisions with other portal

            // If the other portal exists...
            // Check if portals are on the same plane (if they are both on the ceiling, both on the floor, or on a wall in a certain direction.
            // If they are, check if they are within a width of eachother, if they are move them, check if they are within a height of eachother, if they are, move them.
            if (newPortal.GetComponent<Portal>().Other())
            {
                Transform otherTrans = newPortal.GetComponent<Portal>().Other().transform;

                // If the new portal and other portal are either both on the floor or both on the ceiling
                bool thisUp = newTrans.forward == Vector3.up;
                bool thisDown = newTrans.forward == Vector3.down;
                bool otherUp = otherTrans.forward == Vector3.up;
                bool otherDown = otherTrans.forward == Vector3.down;


                if (GameManager.instance.debug)
                    Debug.Log("thisUp: " + thisUp + "thisDown: " + thisDown + "otherUp: " + otherUp + "otherDown: " + otherDown);


                if ((thisUp && otherUp) || (thisDown && otherDown))
                {
                    // Make sure they are not intersecting.
                }
                // If the new portal and other portal are on the vertical plane.
                else if (newTrans.forward == otherTrans.forward)
                {
                    if (GameManager.instance.debug)
                        Debug.Log("on same plane as other portal");

                    if (GameManager.instance.debug)
                        Debug.Log("Pos new " + newTrans.position + " Pos Other " + otherTrans.position);


                    // If the difference in vertical distance is less than the height of the portals, move the new one by that much in the opposite direction.
                    float diffY = otherTrans.position.y - newTrans.position.y;
                    if (GameManager.instance.debug)
                        Debug.Log("difference in Y is " + otherTrans.position.y + " - " + newTrans.position.y + " = " + diffY);
                    // If the portal is not near the other portal, return.
                    if (Mathf.Abs(diffY) > ver)
                        continue;




                    // If the difference should be on the X plane, calculate X plane movement.
                    if ((otherTrans.forward == Vector3.forward && newTrans.forward == Vector3.forward) || (otherTrans.forward == Vector3.back && newTrans.forward == Vector3.back))
                    {
                        // The horizontal difference between the two positions on X axis.
                        float diffHor = otherTrans.position.x - newTrans.position.x;

                        if (GameManager.instance.debug)
                            Debug.Log("difference in Hor is " + otherTrans.position.x + " - " + newTrans.position.x + " = " + diffHor);

                        // If the portal is not near the other portal, return.
                        if (Mathf.Abs(diffHor) > hor)
                            continue;

                        // If the code has made it this far, the portal has moved.
                        moved = true;

                        // If the horizontal difference is proportionately greater than the vertical difference, move horizontally.
                        if (Mathf.Abs(diffHor) >= .5 * Mathf.Abs(diffY))
                        {
                            // If the new portal is to the right of the other portal, move right.
                            if (diffHor < 0)
                                newTrans.Translate(newTrans.right * (hor + diffHor));
                            // If the new portal is to the left of the other portal, move left.
                            else
                                newTrans.Translate(-newTrans.right * (hor - diffHor));
                        }
                        // If the vertical difference is proportionately greater than the horizontal difference, move vertically.
                        else
                        {
                            // If the new portal is above the other portal.
                            if (diffY < 0)
                                newTrans.Translate(newTrans.up * (ver + diffY));
                            // If the new portal is below the other portal.
                            else
                                newTrans.Translate(-newTrans.up * (ver - diffY));
                        }
                    } // If the difference should be on the Z plane, calculate Z plane movement.
                    else
                    {
                        // The horizontal difference between the two positions on Z axis.
                        float diffHor = otherTrans.position.z - newTrans.position.z;
                        if (GameManager.instance.debug)
                            Debug.Log("difference in Hor is " + otherTrans.position.z + " - " + newTrans.position.z + " = " + diffHor);

                        // If the portal is not near the other portal, return.
                        if (Mathf.Abs(diffHor) > hor)
                        {
                            return;
                        }

                        // If the horizontal difference is proportionately greater than the vertical difference, move horizontally.
                        if (Mathf.Abs(diffHor) >= .5 * Mathf.Abs(diffY))
                        {
                            // If the new portal is to the right of the other portal, move right.
                            if (diffHor < 0)
                                newTrans.Translate(-newTrans.forward * (hor + diffHor));
                            // If the new portal is to the left of the other portal, move left.
                            else
                                newTrans.Translate(newTrans.forward * (hor - diffHor));
                        }
                        // If the vertical difference is proportionately greater than the horizontal difference, move vertically.
                        else
                        {
                            // If the new portal is above the other portal.
                            if (diffY < 0)
                                newTrans.Translate(newTrans.up * (ver + diffY));
                            // If the new portal is below the other portal.
                            else
                                newTrans.Translate(-newTrans.up * (ver - diffY));
                        }
                    }
                }
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

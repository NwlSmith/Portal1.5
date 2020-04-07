using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalShootingAlt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject portal;
    public float speed = 50;

    public float length = 1000f;
    public GameObject aimer;

    private Camera cam;
    private bool portalDelay;

    void Update()
    {

        // float mouseX = Input.GetAxis("Mouse X");
        // float mouseY = Input.GetAxis("Mouse Y");
        // transform.Rotate(0, mouseX * 5, 0);

        // Camera.main.transform.Rotate(-mouseY * 5, 0, 0);


        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(myRay.origin, myRay.direction * length, Color.red);
        RaycastHit myHit;

        if (Physics.Raycast(myRay, out myHit, length))
        {
            aimer.transform.position = myHit.point;
            if (Input.GetMouseButtonDown(0) && !portalDelay && myHit.collider.gameObject.tag == "CanHoldPortals")
            {
                Debug.Log("hit pos " + myHit.point + " normal " + myHit.normal);
                StartCoroutine(delayPortal());
                GameObject insBall = Instantiate(portal);
                insBall.transform.SetParent(null);
                insBall.transform.forward = myHit.normal;
                insBall.transform.position = myHit.point + .01f * myHit.normal;
                Debug.Log("Portal pos " + insBall.transform.position + " normal " + insBall.transform.forward);
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

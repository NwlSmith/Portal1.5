using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 4/2/2020
 * Creator: Mark Timchenko
 * 
 * Description: Allows the player to shoot Blue Portals.
 */
public class portalShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject portal;
    public float speed = 50;

    public float length = 1000f;
    public GameObject aimer;

    private Camera cam;
    private bool portalDelay;

    
 
    void Start () {

        portal = PortalManager.instance.bluePrefab;
        // ball = GetComponent<GameObject>();
    }
 
    void Update(){
        
       // float mouseX = Input.GetAxis("Mouse X");
       // float mouseY = Input.GetAxis("Mouse Y");
       // transform.Rotate(0, mouseX * 5, 0);

       // Camera.main.transform.Rotate(-mouseY * 5, 0, 0);
        
        
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        Debug.DrawRay(myRay.origin,myRay.direction*length,Color.red);
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
        
        if (Physics.Raycast(myRay, out myHit, length))
        {
            // myHit.transform.Rotate(1,0,0);
            aimer.transform.position = myHit.point;
            //if (myHit.collider.gameObject.name != "wallNo")
           // {
                if (Input.GetMouseButtonDown(0) && !portalDelay)
                {
                    StartCoroutine(delayPortal());
                     GameObject insBall = Instantiate(portal);
                    insBall.transform.SetParent(null);
                    insBall.transform.forward = myHit.normal;
                    insBall.transform.position = myHit.point + .01f * myHit.normal;
                   
            
                }
                
                
                
          //  }
          

        }
    }

    IEnumerator delayPortal()
    {
        portalDelay = true;
        yield return new WaitForSeconds(.05f);
        portalDelay = false;
    }
}

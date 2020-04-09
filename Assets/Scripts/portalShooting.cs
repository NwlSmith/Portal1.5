using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script shoots portals.  it uses a raycast to detect objects and throw portals.
//it also handles the crosshairs which swap depending on which portal is out 
public class PortalShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject portal;
    public GameObject portalRight;
    public float speed = 50;

    public float length = 1000f;
    public GameObject aimer;
    //left portal is the blue one
    public bool leftPortal;
    //right portal is the orange one
    public bool rightPortal;
    private Camera cam;
    private bool portalDelay;

    
 
    void Start () {
        

        // ball = GetComponent<GameObject>();
    }
 
    void Update(){
        
     
        
        
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        Debug.DrawRay(myRay.origin,myRay.direction*length,Color.red);
        RaycastHit myHit;

      
        //constantly check for collisions through rayCast
        if (Physics.Raycast(myRay, out myHit, length))
        {
            aimer.transform.position = myHit.point;
            //left click to shoot the blue portal
                if (Input.GetMouseButtonDown(0) && !portalDelay)
                {
                    //delay portal explained below in its coroutine
                    StartCoroutine(DelayPortal());
                     GameObject insBall = Instantiate(portal);
                    insBall.transform.SetParent(null);
                    insBall.transform.rotation = transform.rotation;
                    insBall.transform.position = this.transform.position;
                   
            
                }
                //right click to shoot orange portal
                if (Input.GetMouseButtonDown(1) && !portalDelay)
                {
                    
                    StartCoroutine(DelayPortal());
                    GameObject insportal2 = Instantiate(portalRight);
                    insportal2.transform.SetParent(null);
                    insportal2.transform.rotation = transform.rotation;
                    insportal2.transform.position = this.transform.position;
                   
            
                }

                
                
                
                
          //  }
          

        }
    }
    //this coroutine delays the portals so you cant spam them right after one another.  
    //change the WaitForSeconds to make a longer delay

    IEnumerator DelayPortal()
    {
        portalDelay = true;
        yield return new WaitForSeconds(.05f);
        portalDelay = false;
    }
}

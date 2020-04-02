using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        

        // ball = GetComponent<GameObject>();
    }
 
    void Update(){
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(0, mouseX * 5, 0);

        Camera.main.transform.Rotate(-mouseY * 5, 0, 0);
        
        
        Cursor.lockState = CursorLockMode.Locked;
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
            
            if (Input.GetMouseButtonDown(0) && !portalDelay)
            {
                StartCoroutine(delayPortal());
                GameObject insBall = Instantiate(portal);
                insBall.transform.SetParent(null);
                insBall.transform.rotation = transform.rotation;
                insBall.transform.position = this.transform.position;
            
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

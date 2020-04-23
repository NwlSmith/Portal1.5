using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date Created: 4/18/2020
 * Creator: Raymond Lothian
 * 
 * Description: Destroys any objects other than the player and closes all portals when passing through.
 */

public class MaterialEmancipationGrill : MonoBehaviour
{
    PortalManager PortalManager;


    // Start is called before the first frame update
    void Start()
    {

        PortalManager = GetComponent<PortalManager>();

    }



    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks that when player passes the collider it destroys all portals any cubes it touches
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Grill is working");
            PortalManager.instance.DestroyPortals();

            if (other.CompareTag("CanPickUp")) {
                other.GetComponent<ObjectUtility>().DestroyMe();
            }
        }
       

    }

}

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



    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks if the object passing through the 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Grill is working");
            

        }
       

    }

}

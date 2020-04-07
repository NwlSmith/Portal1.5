using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalWallDisable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Portal entered " + name);
            // Change this to make it so it's only false to that object? Restructure the collider?
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Change this to make it so it's only false to that object? Restructure the collider?
            GetComponentInParent<Portal>().surface.GetComponent<Collider>().enabled = true;
        }
    }
}

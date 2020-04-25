using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughLevelFailsafe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = new Vector3(0f, 10f, 0f);
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.physicsVector = new Vector3(0f, 2f, 0f);
            playerMovement.gameObject.layer = 21;
        }
        if (other.TryGetComponent(out ObjectUtility objectUtility))
        {
            objectUtility.GetComponent<Rigidbody>().velocity = new Vector3(0f, 2f, 0f);
            objectUtility.gameObject.layer = 20;
        }
    }
}

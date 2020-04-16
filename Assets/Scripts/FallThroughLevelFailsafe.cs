using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughLevelFailsafe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.Translate(0f, 10f, 0f);
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.physicsVector = new Vector3(0f, 10f, 0f);
        }
    }
}

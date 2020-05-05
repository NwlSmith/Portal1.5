using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudio : MonoBehaviour
{
    private AudioSource aS;
    private PlayerMovement pM;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
        pM = GetComponentInParent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        float fraction = (pM.physicsVector.magnitude - pM.moveSpeed) / (pM.maxVelocity - pM.moveSpeed);
        fraction = Mathf.Clamp(fraction, 0f, 1f);
        if (GameManager.instance.debug)
            Debug.Log("velocity = " + pM.physicsVector.magnitude + " fraction = " + fraction);
        aS.volume = Mathf.SmoothStep(0f, 1f, fraction);
    }
}

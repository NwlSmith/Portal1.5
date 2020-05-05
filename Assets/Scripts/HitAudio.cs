using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAudio : MonoBehaviour
{
    private float lastSpeed;
    private AudioSource aS;
    private PlayerMovement pM;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
        pM = GetComponentInParent<PlayerMovement>();
    }

    private void FixedUpdate()
    {

        if (lastSpeed - pM.physicsVector.magnitude > 10f)
        {
            aS.pitch = Random.Range(.5f, 1.25f);
            aS.Play();
        }
        lastSpeed = pM.physicsVector.magnitude;
    }
}

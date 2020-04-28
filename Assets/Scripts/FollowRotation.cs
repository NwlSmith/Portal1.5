using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{

    public Transform target;
    public float speed = 10f;
    private Quaternion lastRotation;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = lastRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, speed * Time.deltaTime);
        lastRotation = transform.rotation;
    }


    public void Teleport()
    {
        lastRotation = transform.rotation;
        transform.rotation = target.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{

    public Transform target;
    public float speed = 10f;
    public Quaternion offset;
    private Quaternion lastRotation;

    private void Start()
    {
        offset = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = lastRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, speed * Time.deltaTime);
        lastRotation = transform.rotation;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    private float forwardBackward;

    private float rightLeft;

    private Vector3 inputVector;

    public float moveSpeed = 5.0f;

    public Rigidbody RB;
// Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(0, mouseX * 5, 0);

        Camera.main.transform.Rotate(-mouseY * 5, 0, 0);
        Cursor.lockState = CursorLockMode.Locked;
        
        forwardBackward = Input.GetAxis("Vertical");
        rightLeft = Input.GetAxis("Horizontal");



        inputVector = transform.forward * forwardBackward;

        inputVector += transform.right * rightLeft;


    }

    private void FixedUpdate()
    {
        RB.velocity = (inputVector * moveSpeed * Time.fixedDeltaTime * 300)+ Physics.gravity*.69f;
    }
}

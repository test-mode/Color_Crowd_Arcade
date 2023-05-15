using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public DynamicJoystick joystick;
    public Transform playerTransform;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        Vector3 direction = (Vector3.forward * joystick.Vertical) + (Vector3.right * joystick.Horizontal);

        rb.velocity = direction * speed * Time.fixedDeltaTime;


        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

    }

}
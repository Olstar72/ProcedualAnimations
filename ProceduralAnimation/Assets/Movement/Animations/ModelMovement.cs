﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMovement : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _rotationDampening = 1;
    [SerializeField] private float _rotationDeadzone = 0.02f;
    [SerializeField] private float _tiltingDampening = 1;
    [SerializeField] private float _tiltingMult = 5;

    public Vector3 acceleration;
    public float accelerationMag;
    private Vector3 previousVelocity;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        previousVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 eulerAngles = transform.parent.localEulerAngles;

        Vector3 velocity = _rb.velocity;
        Vector3 horizontalVelocity = new Vector3(velocity.z, 0, -velocity.x);

        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + horizontalVelocity, Color.red);
        
        //acceleration = horizontalVelocity - previousVelocity;
        //previousVelocity = horizontalVelocity;

        Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + acceleration, Color.green);
        //accelerationMag = acceleration.magnitude;

        // Tilting
        if (eulerAngles.x > 180)
            eulerAngles.x = eulerAngles.x - 360;
        if (eulerAngles.z > 180)
            eulerAngles.z = eulerAngles.z - 360;

        float horizontalAngle = Mathf.Lerp(eulerAngles.x, -acceleration.x * _tiltingMult, Time.deltaTime * _tiltingDampening);
        float verticalAngle = Mathf.Lerp(eulerAngles.z, -acceleration.z * _tiltingMult, Time.deltaTime * _tiltingDampening);

        eulerAngles = new Vector3(horizontalAngle, eulerAngles.y, verticalAngle);

        transform.parent.localEulerAngles = eulerAngles;

        // Facing Velocity
        if (horizontalVelocity.magnitude > _rotationDeadzone)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(horizontalVelocity, Vector3.up), Time.deltaTime * _rotationDampening);
    }
}

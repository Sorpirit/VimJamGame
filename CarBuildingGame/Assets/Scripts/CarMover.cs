using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    [SerializeField] private WheelJoint2D[] motorWheels;

    private float moveFactor;
    private float prevMoveFactor;
    private bool drive;

    private void Start()
    {
        drive = false;
        UpdateWheelsState();
    }

    private void Update()
    {
        moveFactor = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if(drive != (moveFactor == 0f))
        {
            drive = moveFactor == 0f;
            UpdateWheelsState();
        }

        
    }

    private void UpdateWheelsState(float speedFactor = 1)
    {
        foreach(WheelJoint2D wheel in motorWheels)
        {
            wheel.useMotor = drive;
            JointMotor2D newMotor = new JointMotor2D { motorSpeed = wheel.motor.motorSpeed * speedFactor, maxMotorTorque = wheel.motor.maxMotorTorque };
            wheel.motor = newMotor;
        }
    }
}

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
        moveFactor = -Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if(prevMoveFactor != moveFactor)
        {
            drive = moveFactor != 0f;

            UpdateWheelsState(drive ? moveFactor : 1);


            prevMoveFactor = moveFactor;
        }

        
    }

    private void UpdateWheelsState(float speedFactor = 1)
    {
        foreach(WheelJoint2D wheel in motorWheels)
        {
            float speed = Mathf.Abs(wheel.motor.motorSpeed) * speedFactor;
            JointMotor2D newMotor = new JointMotor2D {motorSpeed = speed, maxMotorTorque = wheel.motor.maxMotorTorque };
            wheel.motor = newMotor;
            wheel.useMotor = drive;
        }
    }
}

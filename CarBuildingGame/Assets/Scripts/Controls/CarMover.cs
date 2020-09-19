using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour,IActiveComponent
{
    [SerializeField] private List<WheelJoint2D> motorWheels;

    private float moveFactor;
    private float prevMoveFactor;
    private bool drive;

    private bool isFrozen;

    private void Start()
    {
        drive = false;
        UpdateWheelsState();
    }

    private void Update()
    {
        if (isFrozen)
            return;

        moveFactor = -Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (isFrozen)
            return;

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

    public void AddMotorWheel(WheelJoint2D wheel)
    {
        motorWheels.Add(wheel);
    }

    public void Freeze(bool val)
    {
        isFrozen = val;
    }
}

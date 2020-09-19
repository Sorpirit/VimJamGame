using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWheel : MonoBehaviour, ICarPart
{
    [SerializeField] private float wheelSpeed;
    [SerializeField] private float wheelSuspention;
    [SerializeField] private GameObject wheelModel;
    [SerializeField] private bool isMotorWheel;
    [SerializeField] private Collider2D partColider; 

    public void AddPart(Vector2 pos, GameObject carObj)
    {
        WheelJoint2D joint = carObj.AddComponent<WheelJoint2D>();
        GameObject wheel = Instantiate(wheelModel,pos,Quaternion.identity,carObj.transform);

        joint.connectedBody = wheel.GetComponent<Rigidbody2D>();
        joint.anchor = pos;

        JointSuspension2D suspension = new JointSuspension2D { frequency = wheelSuspention, angle = 90, dampingRatio = .7f };
        joint.suspension = suspension;

        JointMotor2D motor = new JointMotor2D { motorSpeed = wheelSpeed, maxMotorTorque = 10000 };
        joint.motor = motor;

        joint.useMotor = false;

        if (isMotorWheel)
        {
            carObj.GetComponent<CarMover>().AddMotorWheel(joint);
        }
    }
}

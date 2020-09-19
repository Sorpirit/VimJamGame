using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketJumper : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float rechargeTime;

    private float fireTimer;

    private void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;
    }

    public void Jump(Rigidbody2D carBody)
    {
        if (fireTimer > 0)
            return;

        fireTimer = rechargeTime;
        carBody.AddForceAtPosition(transform.up * force, transform.position);
    }
}

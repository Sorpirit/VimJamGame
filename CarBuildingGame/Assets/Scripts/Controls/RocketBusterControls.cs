using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBusterControls : MonoBehaviour,IActiveComponent
{

    private List<RocketJumper> jumpers;
    private Rigidbody2D carBody;

    private bool isFrozen;

    private void Awake()
    {
        carBody = GetComponent<Rigidbody2D>();
        jumpers = new List<RocketJumper>();

        isFrozen = true;
    }

    private void Update()
    {
        if (isFrozen)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

    private void Jump()
    {
        foreach(RocketJumper jumper in jumpers)
        {
            jumper.Jump(carBody);
        }
    }

    public void AddBuster(RocketJumper jumper)
    {
        jumpers.Add(jumper);
    }

    public void Freeze(bool val)
    {
        isFrozen = val;
    }
}

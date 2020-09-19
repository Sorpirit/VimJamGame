using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBusterEdit : MonoBehaviour,ICarPart
{
    [SerializeField] private GameObject rocketJumper;
    public void AddPart(Vector2 pos, GameObject carObj)
    {
        GameObject part = Instantiate(rocketJumper, pos, Quaternion.identity, carObj.transform);
        RocketJumper jumper = part.GetComponent<RocketJumper>();
        RocketBusterControls controls = carObj.GetComponent<RocketBusterControls>();

        if (controls == null)
            controls = carObj.AddComponent<RocketBusterControls>();

        controls.AddBuster(jumper);
    }
}

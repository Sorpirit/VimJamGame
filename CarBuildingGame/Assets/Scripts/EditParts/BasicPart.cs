using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPart : MonoBehaviour,ICarPart
{
    [SerializeField] private GameObject carPart;
    public void AddPart(Vector2 pos, GameObject carObj)
    {
        Instantiate(carPart, pos, Quaternion.identity, carObj.transform);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartButton : MonoBehaviour
{
    [SerializeField] private GameObject carPart;
    [SerializeField] private CarEdit edit;

    public void PeekPart()
    {
        Debug.Log("Peek");
        edit.Select(Instantiate(carPart,edit.transform));
    }
}

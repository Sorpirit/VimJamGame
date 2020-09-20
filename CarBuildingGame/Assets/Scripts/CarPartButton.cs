using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartButton : MonoBehaviour
{
    public GameObject carPart;
    private CarEdit edit;

    private void Awake()
    {
        edit = FindObjectOfType<CarEdit>();
    }

    public void PeekPart()
    {
        edit.Select(Instantiate(carPart,edit.transform));
    }
}

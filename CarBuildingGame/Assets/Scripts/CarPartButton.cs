using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartButton : MonoBehaviour
{
    public GameObject carPart;
    private CarEdit edit;
    
    public delegate void ClickAction(GameObject go);
    public event ClickAction OnClicked;

    private void Awake()
    {
        edit = FindObjectOfType<CarEdit>();
    }

    public void PeekPart()
    {
        OnClicked(carPart);
        edit.Select(Instantiate(carPart,edit.transform));
    }
}

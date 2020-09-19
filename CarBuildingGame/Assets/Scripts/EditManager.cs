using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private CarEdit edit;
    [SerializeField] private bool autoStartEditing;
    public delegate void MakeEdit();
    public static event MakeEdit MakeEdditing;

    private bool isInEditingMode;
    public bool EditingIsPossible;

    //Experience is a basic property
    public bool IsInEditingMode { get { return isInEditingMode; } }

    private void Start()
    {
        if (autoStartEditing)
        {
            isInEditingMode = true;
            StartOrEndEditing();
        }
        else
        {
            isInEditingMode = false;
            StartOrEndEditing();
        }
        MakeEdditing += StartOrEndEditing;
    }

    public void StartOrEndEditing()
    {
        if (isInEditingMode) 
        {
            car.GetComponent<Rigidbody2D>().isKinematic = true;
            FreezeActiveComponents(true);

            edit.gameObject.SetActive(true);

            isInEditingMode = false;
        }
        else
        {
            car.GetComponent<Rigidbody2D>().isKinematic = false;
            FreezeActiveComponents(false);

            edit.Deselect();
            edit.AttachAllComponents();

            edit.gameObject.SetActive(false);

            isInEditingMode = true;
        }
    }

    private void FreezeActiveComponents(bool val)
    {
        IActiveComponent[] activeComp = car.GetComponents<IActiveComponent>();
        foreach(IActiveComponent component in activeComp)
        {
            component.Freeze(val);
        }
    }
}

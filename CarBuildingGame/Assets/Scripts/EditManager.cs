﻿using System;
using UnityEngine;

public class EditManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject editPanel;
    [SerializeField] private CarEdit edit;
    [SerializeField] private bool autoStartEditing;
    [SerializeField] private GameObject carPrefab;
    private bool isInEditingMode;
    private bool rebuildCar;

    public Action<GameObject> OnCarChenged;
    private void Start()
    {
        if (autoStartEditing)
            StartEditing();
    }

    public void StartEditing()
    {
        if (rebuildCar)
        {
            Destroy(car);
            car = Instantiate(carPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            OnCarChenged?.Invoke(car);
        }


        Rigidbody2D carRb = car.GetComponent<Rigidbody2D>();
        carRb.isKinematic = true;
        car.transform.position = Vector2.zero;

        FreezeActiveComponents(true);

        editPanel.SetActive(true);
        edit.gameObject.SetActive(true);

        isInEditingMode = true;
    }

    public void EndEditing()
    {
        car.GetComponent<Rigidbody2D>().isKinematic = false;
        FreezeActiveComponents(false);

        edit.Deselect();
        edit.AttachAllComponents();

        editPanel.SetActive(false);
        edit.gameObject.SetActive(false);

        isInEditingMode = false;
        rebuildCar = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInEditingMode)
                EndEditing();
            else
                StartEditing();
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

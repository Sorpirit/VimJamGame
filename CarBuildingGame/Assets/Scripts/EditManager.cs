using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject editPanel;
    [SerializeField] private CarEdit edit;
    [SerializeField] private bool autoStartEditing;

    private bool isInEditingMode;

    private void Start()
    {
        if (autoStartEditing)
            StartEditing();
    }

    public void StartEditing()
    {
        car.GetComponent<Rigidbody2D>().isKinematic = true;
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

        editPanel.SetActive(false);
        edit.gameObject.SetActive(false);

        isInEditingMode = false;
    }

    private void Update()
    {
        if (!isInEditingMode)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndEditing();
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

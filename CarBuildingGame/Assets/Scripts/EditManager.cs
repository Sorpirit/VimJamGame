 using System;
using Cinemachine;
using UnityEngine;

public class EditManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private CarEdit edit;
    [SerializeField] private bool autoStartEditing;
    [SerializeField] private GameObject carPrefab;
    private bool isInEditingMode;
    private bool rebuildCar;

    public bool IsInEditingMode => isInEditingMode;
    public bool ModeChangesOnSpaceBar = true;
    public Action OnEnterEditing;
    public Action OnExitEditing;
    public Action<GameObject> OnCarChenged;
    public Transform GaragePlaceholder;

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
        car.transform.position = GaragePlaceholder.transform.position;

        FreezeActiveComponents(true);

        edit.gameObject.SetActive(true);

        isInEditingMode = true;
        OnEnterEditing?.Invoke();
    }
    public void EndEditing()
    {
        car.GetComponent<Rigidbody2D>().isKinematic = false;
        FreezeActiveComponents(false);

        edit.Deselect();
        edit.AttachAllComponents();

        edit.gameObject.SetActive(false);

        isInEditingMode = false;
        rebuildCar = true;
        OnExitEditing?.Invoke();
    }

    private void Update()
    {
        if (ModeChangesOnSpaceBar)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isInEditingMode)
                    EndEditing();
                else
                    StartEditing();
            }
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

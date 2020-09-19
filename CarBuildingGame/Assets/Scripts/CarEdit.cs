using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEdit : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private Camera cam;

    private GameObject selectedCarPart;
    private bool isDraged;
    private bool isAbelToPlace;

    private void Update()
    {
        if (!isDraged)
            return;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        selectedCarPart.transform.position = mousePos;

        isAbelToPlace = CastPart(mousePos);

        if (Input.GetMouseButtonUp(0))
        {
            TryToAttachPart();
        }
    }
    
    private bool CastPart(Vector2 point)
    {
        Collider2D col = Physics2D.OverlapPoint(point);
        return col != null && col.gameObject == car;
    }

    private void TryToAttachPart()
    {
        if (isAbelToPlace)
        {
            ICarPart part = selectedCarPart.GetComponent<ICarPart>();
            part.AddPart(selectedCarPart.transform.position, car);
        }

        Deselect();
    }

    public void Select(GameObject carPart)
    {
        if (isDraged)
            Deselect();

        isDraged = true;
        selectedCarPart = carPart;
    }

    public void Deselect()
    {
        if (!isDraged)
            return;

        isDraged = false;
        Destroy(selectedCarPart);
    }
}

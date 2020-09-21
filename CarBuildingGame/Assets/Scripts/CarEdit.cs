using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEdit : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private Camera cam;
    [SerializeField] private ContactFilter2D partFilter;
    [SerializeField] private Color cantPlace;
    [SerializeField] private Color canPlace;
    [SerializeField] private Color higlited;
    [SerializeField] private LayerMask carMask;
    [SerializeField] private EditManager edit;

    private GameObject selectedCarPart;
    private bool isDraged;
    private bool isAbelToPlace;
    private ICarPart part;

    private List<ICarPart> carParts;

    private ICarPart lastHiglited;
    private Collider2D lastHiglitedColider;

    private void Start()
    {
        carParts = new List<ICarPart>();
        edit.OnCarChenged += (car) => this.car = car;

    }

    private void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (isDraged)
        {
            
            selectedCarPart.transform.position = mousePos;

            isAbelToPlace = CurssorCast(mousePos) && !part.CastColider(partFilter);
            part.HighlightSprite(isAbelToPlace ? canPlace : cantPlace);

            if (Input.GetMouseButtonUp(0))
            {
                TryToAttachPart();
            }
        }
        else
        {
            Collider2D col = Physics2D.OverlapPoint(mousePos, partFilter.layerMask);
            if (col != null && col != lastHiglitedColider && col.TryGetComponent(out ICarPart carPart))
            {
                if (lastHiglited != null)
                    lastHiglited.HighlightSprite(Color.white);
                carPart.HighlightSprite(higlited);

                lastHiglited = carPart;
                lastHiglitedColider = col;
            }
            else if(lastHiglitedColider != col)
            {
                lastHiglited.HighlightSprite(Color.white);
                lastHiglited = null;
                lastHiglitedColider = null;
            }

            if(lastHiglitedColider != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraged = true;
                    lastHiglited.HighlightSprite(Color.white);
                    part = lastHiglited;
                    selectedCarPart = lastHiglitedColider.gameObject;
                    carParts.Remove(lastHiglited);

                    lastHiglited = null;
                    lastHiglitedColider = null;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    carParts.Remove(lastHiglited);
                    lastHiglited.DeletePart();

                    lastHiglited = null;
                    lastHiglitedColider = null;
                }

            }
        }
    }

    private void TryToAttachPart()
    {
        if (isAbelToPlace)
        {
            carParts.Add(part);
            part.HighlightSprite(Color.white);
            isDraged = false;
            selectedCarPart = null;
            part = null;
        }
        else
        {
            Deselect();
        }
    }

    private bool CurssorCast(Vector2 pos)
    {
        return Physics2D.OverlapPoint(pos, carMask);
    }

    public void Select(GameObject carPart)
    {
        if (isDraged)
            Deselect();

        isDraged = true;
        selectedCarPart = carPart;
        part = selectedCarPart.GetComponent<ICarPart>();
    }

    public void Deselect()
    {
        if (!isDraged)
            return;

        isDraged = false;
        Destroy(selectedCarPart);
    }

    public void AttachAllComponents()
    {
        if (carParts != null)
        {
            foreach(ICarPart part in carParts)
            {
                part.AttachPart(car);
            }
        }
    }
}

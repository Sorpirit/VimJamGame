using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPart : MonoBehaviour,ICarPart
{
    [SerializeField] private GameObject carPart;
    [SerializeField] private Collider2D partColider;
    [SerializeField] private SpriteRenderer partSprite;
    public void AttachPart(GameObject carObj)
    {
        Instantiate(carPart, transform.position, Quaternion.identity, carObj.transform);
    }

    public bool CastColider(ContactFilter2D filter)
    {
        return partColider != null && partColider.OverlapCollider(filter, new Collider2D[2]) > 0;
    }

    public void HighlightSprite(Color tint)
    {
        partSprite.color = tint;
    }

    public void DeletePart()
    {
        Destroy(gameObject);
    }
}

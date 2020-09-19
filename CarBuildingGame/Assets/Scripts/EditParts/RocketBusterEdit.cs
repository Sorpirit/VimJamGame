using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBusterEdit : MonoBehaviour,ICarPart
{
    [SerializeField] private GameObject rocketJumper;
    [SerializeField] private Collider2D partColider;
    [SerializeField] private SpriteRenderer partSprite;
    public void AttachPart(GameObject carObj)
    {
        GameObject part = Instantiate(rocketJumper, transform.position, Quaternion.identity, carObj.transform);
        RocketJumper jumper = part.GetComponent<RocketJumper>();
        RocketBusterControls controls = carObj.GetComponent<RocketBusterControls>();

        if (controls == null)
            controls = carObj.AddComponent<RocketBusterControls>();

        controls.AddBuster(jumper);
    }

    public bool CastColider(ContactFilter2D filter)
    {
        return partColider.OverlapCollider(filter, new Collider2D[2]) > 0;
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

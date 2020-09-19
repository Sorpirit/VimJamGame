using UnityEngine;

public interface ICarPart
{
    //void SetPartPosition(Vector2 pos);
    void AttachPart(GameObject carObj);

    bool CastColider(ContactFilter2D filter);

    void HighlightSprite(Color tint);
    void DeletePart();
}

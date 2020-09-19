using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class textMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        transform.DOMoveX(transform.position.x, 7);
    }
}

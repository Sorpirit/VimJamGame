using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    [SerializeField] private float timeExistence;
    [SerializeField] private GameObject PlaceHolder;
    private void Update()
    {
        
    }

    private void Start()
    {
        timeExistence = 7;
        DOTween.Init();
        transform.DOMoveX(PlaceHolder.transform.position.x, timeExistence);
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(timeExistence);
        Destroy(gameObject.GetComponentInParent<RectTransform>().gameObject);
    }
}

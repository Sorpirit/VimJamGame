using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager: MonoBehaviour
{
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject Fly;
    void Update () {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) 
            {
                if (hit.collider.gameObject.tag == "hat")
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
    }

    private void Start()
    {
        DOTween.Init();
        StartCoroutine(waitForButton());
        StartCoroutine(waitForFly());
    }

    IEnumerator waitForButton()
    {
        yield return new WaitForSeconds(1.4f);
        GameObject go = Instantiate(PlayButton);
        yield return new WaitForSeconds(5.6f);
        StartCoroutine(waitForButton());
        yield return new WaitForSeconds(10);
        Destroy(go);
    }

    IEnumerator waitForFly()
    {
        GameObject go = Instantiate(Fly, parent: MainPanel.transform);
        yield return new WaitForSeconds(7);
        StartCoroutine(waitForFly());
    }
}

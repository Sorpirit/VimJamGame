using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager: MonoBehaviour
{
    [SerializeField] private GameObject BlackPanel;
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
                    StartCoroutine(goToNextScene());
                }
            }
        }
    }

    private void Start()
    {
        DOTween.Init();
        StartCoroutine(waitForButton());
        StartCoroutine(waitForFly());
        StartCoroutine(Opening());
    }

    IEnumerator waitForButton()
    {
        yield return new WaitForSeconds(1.8f);
        GameObject go = Instantiate(PlayButton);
        yield return new WaitForSeconds(5.2f);
        StartCoroutine(waitForButton());
        yield return new WaitForSeconds(10);
        Destroy(go);
    }

    IEnumerator waitForFly()
    {
        GameObject go = Instantiate(Fly, parent: MainPanel.transform);
        yield return new WaitForSeconds(7);
        Destroy(go);
        StartCoroutine(waitForFly());
    }

    IEnumerator goToNextScene()
    {
        BlackPanel.SetActive(true);
        BlackPanel.GetComponent<CanvasGroup>().DOFade(1, 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
    IEnumerator Opening()
    {
        BlackPanel.GetComponent<CanvasGroup>().DOFade(0, 1);
        yield return new WaitForSeconds(1);
        BlackPanel.SetActive(false);
    }
}

















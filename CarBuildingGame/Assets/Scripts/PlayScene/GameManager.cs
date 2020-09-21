using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

/*
 * this is a GameManager and a UIManager at the same time
 */
public class GameManager : MonoBehaviour
{
    /*
     * this first part is takes care of Game managment
     */
    [SerializeField] private EditManager editManager;
    [SerializeField] private GameObject BlackPanel;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject CarPartButton;
    [SerializeField] private float enterAndExitTime;
    [SerializeField] private float spawnAndDespawnUIElementsInGarageSpeed;
    [SerializeField] private List<GameObject> PlceholderPanels;
    [SerializeField] private List<GameObject> Garages;
    [SerializeField] private CarEdit CarEdit;
    
    [HideInInspector] public List<int> ComponentNum;
    public List<GameObject> Components;
    private int Currentgarage;
    private List<Transform> Plceholders;

    [HideInInspector] public List<int> availablePlaceholders;
    private List<int> BackupAvailablePlaceholders;
    public bool DebugMode;
    private bool CurrentlyDragingAComponentFromTheMenu;
    private bool AreEditButtonsDown;
    private bool PutDownCarComponentForTheFirstTime;
    
    // Temp Variables !!don't mess with this!!
    private List<GameObject> TempGameObjectsOnCar;
    private List<GameObject> TempGameObjectsOnCa2;
    
    // Start is called before the first frame update
    void Start()
    {
        PutDownCarComponentForTheFirstTime = true;
        TempGameObjectsOnCa2 = new List<GameObject>();
        TempGameObjectsOnCar = new List<GameObject>();
        ComponentNum = new List<int>();
        Currentgarage = 0;
        EnterNewGarage();
        foreach (GameObject comp in Components)
        {
            ComponentNum.Add(0);
        }
        DOTween.Init();
        editManager.OnEnterEditing += OpenEditMode;
        editManager.OnExitEditing += CloseEditMode;
        OpenScene();
        AddToComponents(3, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (editManager.IsInEditingMode)
        {

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);
                if (hit)
                {
                    foreach (GameObject go in Components)
                    {
                        if (go.name == hit.transform.gameObject.name.Replace("(Clone)", ""))
                        {
                            AddToComponents(Components.IndexOf(go), 1);
                            CloseEditMode();
                            OpenEditMode();
                        }
                    }
                    print(hit.transform.gameObject.name);
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                WhenComponentAdded();
                if (CurrentlyDragingAComponentFromTheMenu)
                {
                    AreEditButtonsDown = false;
                    CurrentlyDragingAComponentFromTheMenu = false;
                    OpenEditMode();
                }
            }
            if (CurrentlyDragingAComponentFromTheMenu)
            {
                if (!AreEditButtonsDown)
                {
                    AreEditButtonsDown = true;
                    CloseEditMode();
                }
            }
        }
    }

    void OpenScene()
    {
        StartCoroutine(waitForOpenScene(enterAndExitTime));
    }

    void CloseScene()
    {
        StartCoroutine(waitForCloseScene(enterAndExitTime));
    }

    public void AddToComponents(int index, int ammount)
    {
        ComponentNum[index] += ammount;
    }

    void OpenEditMode()
    {
        
        foreach (int Component in ComponentNum)
        {
            if (Component >= 1)
            {
                StartCoroutine(makeCarPartButton(spawnAndDespawnUIElementsInGarageSpeed, Components[ComponentNum.IndexOf(Component)]));
            }
        }
        
    }

    void CloseEditMode()
    {
        foreach (Transform t in MainPanel.transform)
        {
            if (t.GetComponent<CarPartButton>() != null)
            {
                StartCoroutine(deleteCarPartButton(t.gameObject, spawnAndDespawnUIElementsInGarageSpeed));
            }
        }

        availablePlaceholders = BackupAvailablePlaceholders;
    }

    public void EnterNewGarage()
    {
        Plceholders = new List<Transform>();
        foreach (Transform holder in PlceholderPanels[Currentgarage].transform)
        {
            Plceholders.Add(holder);
        }

        foreach (Transform holder in Plceholders)
        {
            BackupAvailablePlaceholders = new List<int>();
            availablePlaceholders.Add(Plceholders.IndexOf(holder));
            BackupAvailablePlaceholders.Add(Plceholders.IndexOf(holder));
            if(DebugMode)
                Debug.Log(Plceholders.IndexOf(holder));
        }
        Currentgarage += 1;
        
        editManager.GaragePlaceholder = Garages[Currentgarage-1].transform;
    }

    private void ComponentHasBeenSelectedFromTheMenu(GameObject go)
    {
        CurrentlyDragingAComponentFromTheMenu = true;
    }

    private void WhenComponentAdded()
    {
        TempGameObjectsOnCar = new List<GameObject>();
        foreach (Transform t in CarEdit.gameObject.transform)
        {
            TempGameObjectsOnCar.Add(t.gameObject);
        }
        
        foreach (GameObject ga in TempGameObjectsOnCa2)
        {
            TempGameObjectsOnCar.Remove(ga);
        }

        foreach (GameObject go in Components)
        {
            foreach (GameObject goo in TempGameObjectsOnCar)
            {
                if (go.name == goo.name.Replace("(Clone)", ""))
                {
                    ComponentNum[Components.IndexOf(go)] -= 1;
                    AddToComponents(ComponentNum[Components.IndexOf(go)], -1);
                }
                
            }
        }
        
        TempGameObjectsOnCa2 = new List<GameObject>();
        foreach (Transform t in CarEdit.gameObject.transform)
        {
            TempGameObjectsOnCa2.Add(t.gameObject);
        }
    }
    /*
     * this second part takes care of UI managment
     */
    IEnumerator waitForOpenScene(float time)
    {
        BlackPanel.GetComponent<CanvasGroup>().DOFade(0, time);
        editManager.ModeChangesOnSpaceBar = false;
        yield return new WaitForSeconds(time);
        editManager.ModeChangesOnSpaceBar = true;
        BlackPanel.SetActive(false);
    }

    IEnumerator waitForCloseScene(float time)
    {
        BlackPanel.SetActive(true);
        BlackPanel.GetComponent<CanvasGroup>().DOFade(1, time);
        editManager.ModeChangesOnSpaceBar = false;
        yield return new WaitForSeconds(time);
    }

    IEnumerator makeCarPartButton(float time, GameObject comp)
    {
        
        int randomNum = Random.Range(0, availablePlaceholders.Count);
        int randomNumReal = availablePlaceholders[randomNum];
        GameObject go = Instantiate(CarPartButton,MainPanel.transform);
        
        go.GetComponent<Image>().sprite = comp.GetComponentInChildren<SpriteRenderer>().sprite;
        go.GetComponentInChildren<TextMeshProUGUI>().text = ComponentNum[Components.IndexOf(comp)].ToString();
        
        go.GetComponent<ClipText>().follow = Plceholders[randomNumReal];
        availablePlaceholders.Remove(randomNum);
        go.transform.localScale = new Vector3(0,0,0);
        go.transform.DOScale(new Vector3(1, 1, 1), time);
        go.GetComponent<CanvasGroup>().DOFade(1, time);
        go.GetComponent<CarPartButton>().carPart = comp;
        go.GetComponent<CarPartButton>().OnClicked += ComponentHasBeenSelectedFromTheMenu;
        yield return new WaitForSeconds(time);
        if(DebugMode)
            Debug.Log("Component button element has been spawned");
    }
    IEnumerator makeUIElementForGarage( GameObject MyElement,float time)
    {
        int randomNum = Random.Range(0, availablePlaceholders.Count);
        int randomNumReal = availablePlaceholders[randomNum];
        GameObject go = Instantiate(MyElement,MainPanel.transform);
        go.GetComponent<ClipText>().follow = Plceholders[randomNumReal];
        availablePlaceholders.Remove(randomNum);
        go.GetComponent<CanvasGroup>().alpha = 0;
        go.transform.localScale = new Vector3(0,0,0);
        go.transform.DOScale(new Vector3(1, 1, 1), time);
        go.GetComponent<CanvasGroup>().DOFade(1, time);
        yield return new WaitForSeconds(time);
        if(DebugMode)
            Debug.Log("UI garage element has been spawned");
    }
    IEnumerator deleteCarPartButton(GameObject go, float time)
    {
        go.transform.DOScale(new Vector3(0, 0, 0), time);
        go.GetComponent<CanvasGroup>().DOFade(0, time);
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}













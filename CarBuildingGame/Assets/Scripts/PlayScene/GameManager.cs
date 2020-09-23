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
    public bool DebugMode;
    
    private bool CurrentlyDragingAComponentFromTheMenu;
    private bool AreEditButtonsDown;
    private bool PutDownCarComponentForTheFirstTime;
    private List<Transform> AvalblePlaceholders;
    
    // Temp Variables !!don't mess with this!!
    private List<GameObject> TempGameObjectsOnCar;
    private List<GameObject> TempGameObjectsOnCa2;
    
    // Start is called before the first frame update
    private void Awake()
    {
        TempGameObjectsOnCa2 = new List<GameObject>();
        TempGameObjectsOnCar = new List<GameObject>();
        ComponentNum = new List<int>();
        foreach (GameObject comp in Components)
        {
            ComponentNum.Add(1);
        }
        EnterNewGarage();
        
    }

    void Start()
    {
        PutDownCarComponentForTheFirstTime = true;
        Currentgarage = 0;
        DOTween.Init();
        editManager.OnEnterEditing += OpenEditMode;
        editManager.OnExitEditing += CloseEditMode;
        OpenScene();
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

    private void LateUpdate()
    {
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
        int i = 0;
        foreach (GameObject Component in Components)
        {
            if (ComponentNum[i] >= 1)
            {
                StartCoroutine(makeCarPartButton(spawnAndDespawnUIElementsInGarageSpeed, Component));
            }

            i += 1;
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
    }

    public void EnterNewGarage()
    {
        AvalblePlaceholders = new List<Transform>();
        AvalblePlaceholders.RemoveRange(0, AvalblePlaceholders.Count);
        foreach (Transform holder in PlceholderPanels[Currentgarage].transform)
        {
            AvalblePlaceholders.Add(holder);
            holder.gameObject.name = "Placeholder";
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
        TempGameObjectsOnCar.RemoveRange(0, TempGameObjectsOnCar.Count);
        
        foreach (Transform t in CarEdit.gameObject.transform)
        {
            TempGameObjectsOnCar.Add(t.gameObject);
        }
        
        foreach (GameObject ga in TempGameObjectsOnCa2)
        {
            TempGameObjectsOnCar.Remove(ga);
        }

        if (TempGameObjectsOnCar.Count == 1)
        {
            foreach (GameObject go in Components)
            {
                if (go.name == TempGameObjectsOnCar[0].name.Replace("(Clone)", ""))
                {
                    ComponentNum[Components.IndexOf(go)] -= 1;
                }
            }
        }

        TempGameObjectsOnCa2 = new List<GameObject>();
        TempGameObjectsOnCar.RemoveRange(0, TempGameObjectsOnCar.Count);
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
        List<Transform> APlace = new List<Transform>();
        APlace.RemoveRange(0, APlace.Count);
        foreach (Transform t in AvalblePlaceholders)
        {
            if(t.gameObject.name == "Placeholder")
                APlace.Add(t);
        }
        int randomNum = Random.Range(0, APlace.Count);
        GameObject go = Instantiate(CarPartButton,MainPanel.transform);
        
        go.GetComponent<Image>().sprite = comp.GetComponentInChildren<SpriteRenderer>().sprite;
        go.GetComponentInChildren<TextMeshProUGUI>().text = ComponentNum[Components.IndexOf(comp)].ToString();
        
        go.GetComponent<ClipText>().follow = APlace[randomNum];
        APlace[randomNum].gameObject.name = "Placeholder Occupied";
        go.transform.localScale = new Vector3(0,0,0);
        go.transform.DOScale(new Vector3(1, 1, 1), time);
        go.GetComponent<CanvasGroup>().DOFade(1, time);
        go.GetComponent<CarPartButton>().carPart = comp;
        go.GetComponent<CarPartButton>().OnClicked += ComponentHasBeenSelectedFromTheMenu;
        yield return new WaitForSeconds(time);
        if(DebugMode)
            Debug.Log("Component button element has been spawned");
    }
    IEnumerator deleteCarPartButton(GameObject go, float time)
    {
        go.GetComponent<ClipText>().follow.gameObject.name = "Placeholder";
        go.transform.DOScale(new Vector3(0, 0, 0), time);
        go.GetComponent<CanvasGroup>().DOFade(0, time);
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}













using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private List<GameObject> componentGameObjects;
    public List<CarComponent> Components;
    private int Currentgarage;
    private List<Transform> Plceholders;
    
    [HideInInspector] private List<int> availablePlaceholders;
    private List<int> BackupAvailablePlaceholders;
    public bool DebugMode;

    public struct CarComponent
    {
        public GameObject component;
        public int numofComponents;

        public void AddNumOfOwned(int value)
        {
            numofComponents += value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject comp in componentGameObjects)
        {
            CarComponent a = new CarComponent();
            a.component = comp;
            a.numofComponents = 0;
            Components.Add(a);
        }
        availablePlaceholders = new List<int>();
        BackupAvailablePlaceholders = new List<int>();
        DOTween.Init();
        editManager.OnEnterEditing += OpenEditMode;
        editManager.OnExitEditing += CloseEditMode;
        EnterNewGarage();
        Currentgarage = 0;
        AddToComponents(1, 2);
        OpenScene();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Components[index].AddNumOfOwned(ammount);
    }

    void OpenEditMode()
    {
        
        foreach (CarComponent Component in Components)
        {
            if (Component.numofComponents >= 1)
            {
                StartCoroutine(makeCarPartButton(spawnAndDespawnUIElementsInGarageSpeed, Component.component));
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
    }

    public void EnterNewGarage()
    {
        Plceholders = new List<Transform>();
        foreach (Transform holder in PlceholderPanels[Currentgarage].transform)
        {
            Plceholders.Add(holder);
            availablePlaceholders.Add(Plceholders.IndexOf(holder));
            BackupAvailablePlaceholders.Add(Plceholders.IndexOf(holder));
        }
        Currentgarage += 1;
        
        editManager.GaragePlaceholder = Garages[Currentgarage-1].transform;
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
        BlackPanel.GetComponent<CanvasGroup>().DOFade(1, time);
        editManager.ModeChangesOnSpaceBar = false;
        yield return new WaitForSeconds(time);
    }

    IEnumerator makeCarPartButton(float time, GameObject comp)
    {
        int randomNum = Random.Range(0, availablePlaceholders.Count);
        int randomNumReal = availablePlaceholders[randomNum];
        GameObject go = Instantiate(CarPartButton,MainPanel.transform);
        go.GetComponent<ClipText>().follow = Plceholders[randomNumReal];
        availablePlaceholders.Remove(randomNum);
        go.transform.localScale = new Vector3(0,0,0);
        go.transform.DOScale(new Vector3(1, 1, 1), time);
        go.GetComponent<CanvasGroup>().DOFade(1, time);
        go.GetComponent<CarPartButton>().carPart = comp;
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













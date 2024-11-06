using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElectricBoxManager : MonoBehaviour, IPuzzle
{
    public Canvas canvas;
    public ImageChange ImageChange;
    public LockManagerCharacter LockCManager;
    public GameObject ClickBlock, Door;
    private bool lockSolved = false;
    private Vector3 CameraPosition;

    [Header("Electric Puzzle")]

    public Sprite ToggleOn, ToggleOff, BulbOn, BulbOff;
    public GameObject ElectricPuzzle;
    public Button[] Toggles;
    private int[] BulbStates = new int[4] { 0, 0, 0, 0 };
    private int BulbNum = 0;
    public List<int> Solution = new List<int> { 3, 1, 2, 4};
    public GameFlowManager GameFlowManager;

    [SerializeField] public bool IsSolved { get; set; }

    public void StartPuzzle()
    {
        Debug.Log("ToyBox Puzzle Started");
        canvas.gameObject.SetActive(true);
        if (lockSolved)
        {
            ClickBlock.SetActive(true);
            ElectricPuzzle.SetActive(true);
            InitElectricBox();
        }
        else
        {
            LockCManager.InitLock();
            ClickBlock.SetActive(false);
            ElectricPuzzle.SetActive(false);
        }  
    }

    public void ExitPuzzle()
    {
        canvas.gameObject.SetActive(false);
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    public void LockSolved()  
    {
        lockSolved = true;
        ClickBlock.SetActive(true);
        //ImageChange.SwitchImage(canvas.gameObject.transform.Find("ElectricBoxImg").gameObject, 1, 2);
        ElectricPuzzle.SetActive(true);
    }

    void Start()
    {
        canvas.gameObject.SetActive(false);
        CameraPosition = Camera.main.gameObject.transform.position;
    }

    public void InitElectricBox()
    {
        BulbNum = 0;
        BulbStates = new int[4] { 0, 0, 0, 0 };
        
        for (int i = 0; i < Toggles.Length; i++)
        {
            Toggles[i].gameObject.GetComponent<UnityEngine.UI.Image>().sprite = ToggleOff;
            foreach (Transform child in Toggles[i].transform)
            {
                child.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = BulbOff;
            }
        }
    }
    public void ToggleClicked()
    {
        int Idx = EventSystem.current.currentSelectedGameObject.name[6] - '0';
        
        if (BulbStates[Idx - 1] == 0)
        {
            BulbNum++;
            BulbStates[Idx - 1] = BulbNum;

            Toggles[Idx - 1].gameObject.GetComponent<UnityEngine.UI.Image>().sprite = ToggleOn;

            if (BulbNum <= 4)
            {
                Toggles[Idx - 1].transform.GetChild(BulbNum -1).gameObject.
                GetComponent<UnityEngine.UI.Image>().sprite = BulbOn;
            }

            if (BulbNum == 4)
            {
                CheckSolution();
            }
        }
        else //켜져있던걸 끔 
        {
            InitElectricBox();
            return;
        }

    }

    private void CheckSolution()
    {
        for (int i = 0; i < BulbStates.Length; i++)
        {
            if (BulbStates[i] != Solution[i])
            {
                Debug.Log("Solution Failed");
                //InitElectricBox();
                return;
            }
        }
        Debug.Log("Solution Succeeded");
        IsSolved = true;
        GameFlowManager.StartDoorRoutine(Door);
        for (int i = 0; i < Toggles.Length; i++)
        {
            Toggles[i].interactable = false;
        }
    }



}

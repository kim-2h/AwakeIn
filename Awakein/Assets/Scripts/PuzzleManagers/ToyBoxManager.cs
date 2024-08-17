using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToyBoxManager : MonoBehaviour, IPuzzle
{
    public LockManagerCharacter LockCManager;
    public GameObject ImageChange, InvenManager, ClickBlock, ToyBoxTop;
    public Button Airplain, Doll;
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    private Vector3 CameraPosition;
    public void StartPuzzle()
    {
        Debug.Log("ToyBox Puzzle Started");
        canvas.gameObject.SetActive(true);
        if (IsSolved)
        {
            ClickBlock.SetActive(true);
            ToyBoxTop.SetActive(false);
        }
        else
        {
            LockCManager.InitLock();
            ToyBoxTop.SetActive(true);
            ClickBlock.SetActive(false);
        }   
    }
    // public void HitBoxAClick()
    // {
    //     Debug.Log("HitBoxA Clicked");
    //     HitBoxA.interactable = false;
    //     Text.text = "you got the Orgel";
    //     InvenManager.GetComponent<InvenManager>().ItemAdder("Orgel");
    //     if (HitBoxH.interactable == false)
    //     {
    //         ImageChange.GetComponent<ImageChange>().SwitchImage(canvas.gameObject.transform.Find("PopWindow").gameObject, 1);
    //         //인벤에 아이템 추가
            
    //         IsSolved = true;
    //     }

    // }
    // public void HitBoxHClick()
    // {
    //     Debug.Log("HitBoxH Clicked");
    //     HitBoxH.interactable = false;
    //     Text.text = "you got the human";
    //     InvenManager.GetComponent<InvenManager>().ItemAdder("Doll");
    //     if (HitBoxA.interactable == false)
    //     {
    //         ImageChange.GetComponent<ImageChange>().SwitchImage(canvas.gameObject.transform.Find("PopWindow").gameObject, 1);
    //         //인벤에 아이템 추가

    //         IsSolved = true;
    //     }

    // }
    private void AirplainClick()
    {
        InvenManager.GetComponent<InvenManager>().ItemAdder("Orgel");
    }
    private void DollClick()
    {
        InvenManager.GetComponent<InvenManager>().ItemAdder("Doll");
    }
    public void LockSolved()
    {
        IsSolved = true;
        ClickBlock.SetActive(true);
        Airplain.gameObject.SetActive(true);
        Airplain.onClick.AddListener(AirplainClick);
        Doll.gameObject.SetActive(true);
        Doll.onClick.AddListener(DollClick);
        ToyBoxTop.SetActive(false);
    }
    public void ExitPuzzle()
    {
        if (canvas.gameObject.activeInHierarchy)
        {
            Debug.Log("ToyBox Puzzle Exit");
            canvas.gameObject.SetActive(false);
            Camera.main.gameObject.transform.position = CameraPosition;
        }

    }
    void Start()
    {
        ClickBlock.SetActive(false);
        canvas.gameObject.SetActive(false);
        IsSolved = false;
        Airplain.gameObject.SetActive(false);
        Doll.gameObject.SetActive(false);
        CameraPosition = Camera.main.gameObject.transform.position;
        //Text = canvas.gameObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        ImageChange.GetComponent<ImageChange>().SwitchImage(canvas.gameObject.transform.Find("PopWindow").gameObject, 0);
    }

}

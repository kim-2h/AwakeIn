using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToyBoxManager : MonoBehaviour, IPuzzle
{
    public GameObject ImageChange;
    public Button HitBoxA, HitBoxH;
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    public TextMeshProUGUI Text;
    private Vector3 CameraPosition;
    public void StartPuzzle()
    {
        Debug.Log("ToyBox Puzzle Started");
        canvas.gameObject.SetActive(true);
        if (IsSolved)
        {
            Text.text = "there is nothing to do here";
        }
        else
        {
            Text.text = "this is ToyBox Puzzle";
        }
        
    }
    public void HitBoxAClick()
    {
        Debug.Log("HitBoxA Clicked");
        HitBoxA.interactable = false;
        Text.text = "you got the airplane";
        if (HitBoxH.interactable == false)
        {
            ImageChange.GetComponent<ImageChange>().SwitchImage(canvas.gameObject.transform.Find("PopWindow").gameObject, 1);
            //인벤에 아이템 추가
            IsSolved = true;
        }

    }
    public void HitBoxHClick()
    {
        Debug.Log("HitBoxH Clicked");
        HitBoxH.interactable = false;
        Text.text = "you got the human";
        if (HitBoxA.interactable == false)
        {
            ImageChange.GetComponent<ImageChange>().SwitchImage(canvas.gameObject.transform.Find("PopWindow").gameObject, 1);
            //인벤에 아이템 추가
            IsSolved = true;
        }

    }
    public void ExitPuzzle()
    {
        if (canvas.gameObject.activeInHierarchy)
        {
            Debug.Log("ToyBox Puzzle Exit");
            canvas.gameObject.SetActive(false);
            Camera.main.gameObject.transform.position = CameraPosition;
            Text.text = "";
        }

    }
    void Start()
    {
        canvas.gameObject.SetActive(false);
        IsSolved = false;
        CameraPosition = Camera.main.gameObject.transform.position;
        Text = canvas.gameObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        ImageChange.GetComponent<ImageChange>().SwitchImage(canvas.gameObject.transform.Find("PopWindow").gameObject, 0);
    }

}

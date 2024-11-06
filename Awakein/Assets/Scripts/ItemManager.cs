using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*아이템을 사용한 효과를 매니징하는 스크립트임. 아이템 사용 자체는 인벤매니저에서 어쩌구..
일단 팝업 띄우는거 여기서함*/
public class ItemManager : MonoBehaviour
{
    public List<GameObject> PopUpList = new List<GameObject>();
    public GameObject GameFlowManager;
    public UINoteManager NoteManager;
    public InvenManager InvenManager;
    public bool ClickNotDrag = true;
    public void OpenPopUp(string Name)
    {
        Debug.Log("OpenPopUp!! " + Name);
        if (ClickNotDrag) //클릭일때만 팝업뜸!! 드래그일땐 뜨면안됨 ㅈ같아짐
        {
            if (NoteManager.ContentsMap.ContainsKey(Name))
            {
                Debug.Log("Clicked note in inven: " + Name);
                NoteManager.ClickContent(Name);
                InvenManager.RemoveItem(Name);
                return;
            }
            GameObject PopUp = null;
            foreach (GameObject popup in PopUpList)
            {
                if (popup.name == Name+"PopUpCanvas")
                {
                    PopUp = popup;
                    PopUp.SetActive(true);
                    if (Name == "PhotoFrame")
                    {
                        var String = GameFlowManager.GetComponent<GameFlowManager>().ReturnDialogue("PhotoFrame");
                        if (String != "") GameFlowManager.GetComponent<GameFlowManager>().DialogueManager.CallRoutine(String);
                    }

                    if (GameFlowManager.GetComponent<GameFlowManager>().PuzzleMap.ContainsKey(Name))
                    {
                        GameFlowManager.GetComponent<GameFlowManager>().PuzzleMap[Name].StartPuzzle();
                    }
                    break;
                }
            }
            if (PopUp == null)
            {
                Debug.Log("No such PopUp found");
            }
        }

    }
    public void OpenPopUp(int Idx)
    {
        string Name = PopUpList[Idx].name;
        if (ClickNotDrag)
        {
            GameObject PopUp = null;
            PopUp = PopUpList[Idx];
            if (PopUp == null)
            {
                Debug.Log("No such PopUp found");
            }
            else
            {
                PopUp.SetActive(true);
                if (GameFlowManager.GetComponent<GameFlowManager>().PuzzleMap.ContainsKey(Name))
                {
                    GameFlowManager.GetComponent<GameFlowManager>().PuzzleMap[Name].StartPuzzle();
                }
            }
        }


    }
    public void ClosePopUp(int Idx)
    {
        GameObject PopUp = null;
        PopUp = PopUpList[Idx];
        if (PopUp == null)
        {
            Debug.Log("No such PopUp found");
        }
        else
        {
            PopUp.SetActive(false);
        }   
    }
   
    void Start()
    {
        ClickNotDrag = true;
    }

    
    void Update()
    {
        
    }
}

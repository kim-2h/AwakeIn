using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class GameFlowManager : MonoBehaviour
{
    public DialogueManager DialogueManager;
    public List<GameObject> PuzzleList;
    public List<Item> ItemList;
    public Dictionary<string, IPuzzle> PuzzleMap = new Dictionary<string, IPuzzle>();
    public Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();
    public Dictionary<string, bool> DialogueMap = new Dictionary<string, bool>();

    public string ReturnDialogue(string _object)
    {
        string Ret = "";
        switch (_object)   
        {
            case "Window":
                if (true)
                {
                    Ret = "It's sunny but why am I here??????????????????????????????????????????????";
                    DialogueMap["WindowSunny"] = true; //대사가 나왔으면 true
                }
                break;
            case "Door":
                Ret = "It's locked!";
                if (!DialogueMap["DoorFirst"])
                {
                    Ret = "Should I escape through this door?";
                    DialogueMap["DoorFirst"] = true;
                }
                else
                {
                    DialogueMap["DoorAlways"] = true;
                }
                break; 
            case "PhotoFrame":
                if (!DialogueMap["RichFamilyPhoto"] && PuzzleMap["BookShelfDrawer"].IsSolved)
                {
                    Ret = "It's a photo of a rich family.\nThey look happy.";
                    DialogueMap["RichFamilyPhoto"] = true;
                }
                break;
            case "FishBowl":
                if (!DialogueMap["FishHungry"] && !ItemMap["FishFood"].InInventory)
                {
                    Ret = "The fish looks hungry. It's swimming around.";
                    DialogueMap["FishHungry"] = true;
                }
                else if (!DialogueMap["FishDumb"] && ItemMap["FishFood"].InInventory)
                {
                    //DialogueManager.PlayDialogue("It says \"Do not feed them too much.\" on the label.");
                    Ret = ("...Hell nah.");
                    DialogueManager.QDialogue.Enqueue("It says \"Do not feed them too much.\" on the label.");
                    DialogueManager.QDialogue.Enqueue("But do fish even know how much they should eat? With their tiny brains?\n");
                    //DialogueMap["FishDumb"] = true;
                }
                break;
            default:
                Ret = null;
                break;


        }
        return Ret;
    }

    public void ChairBreaking()
    {
        if (ItemMap["Chair"].InInventory && (PuzzleMap["PhotoFrame"].IsSolved || ItemMap["PhotoFrame"].InInventory)&& 
        (PuzzleMap["Radio"].IsSolved || ItemMap["Radio"].InInventory) && (PuzzleMap["Clock"].IsSolved
        || ItemMap["Clock_Key"].InInventory))
        {
            DialogueManager.CallRoutine("The chair is broken...");
            DialogueMap["ChairBroken"] = true;
        }
        else return;
    }

    void Start()
    {
   
    }
    public void PrintProgress()
    {
        /*아무 버튼이랑 연결해서 모든 퍼즐의 완료 여부를 출력함. 디버그용*/
        Debug.Log("===== ~~Printing Progress~~ =====");
        foreach (KeyValuePair<string, IPuzzle> puzzle in PuzzleMap)
        {
            Debug.Log(puzzle.Key + " : " + puzzle.Value.IsSolved);
        }
        Debug.Log("=================================");
        Debug.Log("===== ~~Printing Item Status~~ =====");
        foreach (KeyValuePair<string, Item> item in ItemMap)
        {
            Debug.Log(item.Key + " : \n"
            + "     Isused :" + item.Value.IsUsed
            + " | InInventory :" + item.Value.InInventory
            + " | IsClickable :" + item.Value.IsClickable);
        }
        Debug.Log("=================================");
    }
    void Update()
    {
        
    }
    void Awake()
    {
        string TempName = "";
        for (int i = 0; i<PuzzleList.Count; i++)
        {
            TempName = PuzzleList[i].gameObject.name;
            TempName = TempName.Replace("Manager", "");
            PuzzleMap.Add(TempName, PuzzleList[i].GetComponent<IPuzzle>());
            if (PuzzleMap[TempName] != null)
            {
                PuzzleMap[TempName].IsSolved = false;
                //Debug.Log(TempName + " Added");
            }
        }
        for (int i = 0; i<ItemList.Count; i++)
        {
            TempName = ItemList[i].ItemName;
            ItemMap.Add(TempName, ItemList[i]);
            if (ItemMap[TempName] != null)
            {
                ItemMap[TempName].IsUsed = false;
                ItemMap[TempName].InInventory = false;
                //Debug.Log(TempName + " Added");
            }
        }

        DialogueMap.Add("ShouldEscape", false);
        DialogueMap.Add("DoorFirst", false);
        DialogueMap.Add("DoorAlways", false);
        DialogueMap.Add("CannotReach", false);
        DialogueMap.Add("ChairBroken", false);
        DialogueMap.Add("GottaUseChair", false);
        DialogueMap.Add("FishHungry", false);
        DialogueMap.Add("FishDumb", false);
        DialogueMap.Add("RichFamilyPhoto", false);
        DialogueMap.Add("BookMany", false);
        DialogueMap.Add("DollShiny", false);
        DialogueMap.Add("WindowSunny", false);


    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class GameFlowManager : MonoBehaviour
{
    public List<GameObject> PuzzleList;
    public List<Item> ItemList;
    public Dictionary<string, IPuzzle> PuzzleMap = new Dictionary<string, IPuzzle>();
    public Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();

    public string ReturnDialogue(string _object)
    {
        string Ret = "";
        switch (_object)   
        {
            case "Window":
                if (!PuzzleMap["Radio"].IsSolved && !ItemMap["GasMask"].InInventory && !ItemMap["Gear"].InInventory)
                {
                    Ret = "I haven't solved any puzzle and got nothing.\n I should look around more.";
                }
                else if (ItemMap["GasMask"].InInventory && ItemMap["Gear"].InInventory)
                {
                    Ret = "At least I got some items...";
                }
                break;
            case "Door":
                Ret = "I need to get out of here!";
                if (PuzzleMap["Radio"].IsSolved && PuzzleMap["Hanoi"].IsSolved)
                {
                    Ret = "I can get out soon!";
                }
                break;
            default:
                Ret = null;
                break;


        }
        return Ret;
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

    }
}

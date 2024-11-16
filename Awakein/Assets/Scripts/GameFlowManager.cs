using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class GameFlowManager : MonoBehaviour
{
    public DialogueManager DialogueManager;
    public InvenManager InvenManager;
    public SoundManager SoundManager;
    public List<GameObject> PuzzleList;
    public List<Item> ItemList;
    public Dictionary<string, IPuzzle> PuzzleMap = new Dictionary<string, IPuzzle>();
    public Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();
    public Dictionary<string, bool> DialogueMap = new Dictionary<string, bool>();

    private bool ToTitle = false;


    public enum EScenes
    {
        Title, Room1, Room2, Room3, Ending
    }
    public EScenes Scenes = EScenes.Title;


    public string ReturnDialogue(string _object)
    {
        string Ret = "";
      
        if (Scenes == EScenes.Room1){
            switch (_object)   
        {
            case "Window":
                if (true)
                {
                    Ret = "It's sunny but why am I here?";
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
                if (ItemMap["PhotoFrame"].InInventory && !DialogueMap["RichFamilyPhoto"] && PuzzleMap["BookShelfDrawer"].IsSolved)
                {
                    Ret = "It's a photo of a rich family.\nThey look happy.";
                    DialogueMap["RichFamilyPhoto"] = true;
                }
                break;
            case "FishBowl":
                if (!DialogueMap["FishHungry"] && !ItemMap["FishFood"].InInventory && !PuzzleMap["FishBowl"].IsSolved)
                {
                    Ret = "The fish looks hungry. It's swimming around.";
                    DialogueMap["FishHungry"] = true;
                }
                else if (!DialogueMap["FishDumb"] && ItemMap["FishFood"].InInventory)
                {
                    //DialogueManager.PlayDialogue("It says \"Do not feed them too much.\" on the label.");
                    Ret = "...Hell nah.";
                    DialogueManager.QDialogue.Enqueue("It says \"Do not feed them too much.\" on the label.");
                    DialogueManager.QDialogue.Enqueue("But do fish even know how much they should eat? With their tiny brains?\n");
                    //DialogueMap["FishDumb"] = true;
                }
                break;
            case "BookShelf":
                if (!DialogueMap["BookMany"] && !PuzzleMap["BookShelf"].IsSolved)
                {
                    Ret = "There are many difficult books here!";
                    DialogueMap["BookMany"] = true;
                }
                break;
            case "Doll":
                if (!DialogueMap["DollShiny"] && !ItemMap["RadioBattery"].InInventory)
                {
                    Ret = "There is someting in the doll...";
                    DialogueMap["DollShiny"] = true;
                }
                break;
            case "Vent":
                if (!PuzzleMap["Vent"].IsSolved)
                {
                    
                }
                else if (PuzzleMap["Vent"].IsSolved && (!ItemMap["OrgelWhole"].InInventory || !ItemMap["FamilyPhoto"].InInventory
                || !(ItemMap["Carpet_Note"].InInventory || ItemMap["Carpet_Note"].IsUsed) ||
                !(ItemMap["Photo_Note"].InInventory || ItemMap["Photo_Note"].IsUsed) || !ItemMap["Gear"].InInventory
                || !ItemMap["GasMask"].InInventory))
                {
                    Ret = "I think I need to look around more...";
                }
                else
                {
                    //Ret = "I can finally get out of here!";
                }
                break;
            default:
                Ret = null;
                break;


        }}
        else if (Scenes ==   EScenes.Room2){
           switch (_object)   
        {
            case "Bird":
                if (!PuzzleMap["BirdCage"].IsSolved){
                    //Debug.Log("BirdNotYetDied");
                Ret="The bird is very restless. I need to calm it down to get that note tied in bird's leg.";
                 DialogueMap["BirdNotKilled"] = true;
                }
                break;
            case "Chemicals":
             if ((!ItemMap["BottleB"].InInventory||!ItemMap["BottleA"].InInventory) || !PuzzleMap["Chemicals"].IsSolved){

                Ret="There's no scale on the beaker?";
                 DialogueMap["ChemicalsNotStarted"] = true;
                
             }   
             break;
              case "BirdCage":
                if (!PuzzleMap["BirdCage"].IsSolved){
                    //Debug.Log("BirdNotYetDied");
                Ret="The bird is very restless. I need to calm it down to get that note tied in bird's leg.";
                 DialogueMap["BirdNotKilled"] = true;
                }
                break;
            case "DoorBack":
            Debug.Log("Ending clicked!!");
                Ret = "Finally I can get out of here!";
            break;   
            default:
            Ret=null;
            break;
            
        }}
        return Ret;
        //
        
    }

    public void VentClicked()
    {

        string Ret = "";
        if (PuzzleMap["Vent"].IsSolved && (!ItemMap["OrgelWhole"].InInventory || !ItemMap["FamilyPhoto"].InInventory
        || !(ItemMap["Carpet_Note"].InInventory || ItemMap["Carpet_Note"].IsUsed) ||
        !(ItemMap["Photo_Note"].InInventory || ItemMap["Photo_Note"].IsUsed) || !ItemMap["Gear"].InInventory
        || !ItemMap["GasMask"].InInventory))
        {
            Ret = "I think I need to look around more...";
            DialogueManager.CallRoutine(Ret);
        }
        else
        {
            Ret = "I can finally get out of here!";
            DialogueManager.CallRoutine(Ret);

        }
    }

    public void ChairBreaking()
    {
        if (ItemMap["Chair"].InInventory && (ItemMap["PhotoFrame"].InInventory || ItemMap["FamilyPhoto"].InInventory
        || ItemMap["Photo_Note"].InInventory)&& 
        (PuzzleMap["Radio"].IsSolved || ItemMap["Radio"].InInventory) && (PuzzleMap["Clock"].IsSolved || ItemMap["Clock_Key"].InInventory)
         && (ItemMap["OrgelBody"].InInventory || ItemMap["Orgel"].InInventory))
        {
            DialogueManager.CallRoutine("The chair is broken...");
            DialogueMap["ChairBroken"] = true;
            InvenManager.RemoveItem("Chair");
        }
        else return;
    }
    public void CannotReach()
    {
        if (!InvenManager.ItemMap["Chair"].InInventory)
        {
            DialogueManager.CallRoutine("I cannot reach it...");
            DialogueMap["CannotReach"] = true;
        }
        else
        {
            DialogueManager.CallRoutine("I think I can reach it with the chair.");
            DialogueMap["GottaUseChair"] = true;
        }
    }

    public void StartDoorRoutine(GameObject Door)
    {
        StartCoroutine(DoorOpen(Door));
        SoundManager.PlaySFX(0);
    }
    IEnumerator DoorOpen(GameObject Door)
    {
        float time = 0;
        Quaternion startRotation = Door.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(Door.transform.rotation.eulerAngles.x, 
        Door.transform.rotation.eulerAngles.y - 30.0f, 
        Door.transform.rotation.eulerAngles.z);
        
        while (time < 5.0f)
        {
            time += Time.deltaTime;
            Door.transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / 5.0f);
            yield return null;
        }
    }

    public bool Room2Test()
    {
                if (PuzzleMap["Vent"].IsSolved && (!ItemMap["OrgelWhole"].InInventory || !ItemMap["FamilyPhoto"].InInventory
                || !(ItemMap["Carpet_Note"].InInventory || ItemMap["Carpet_Note"].IsUsed) ||
                !(ItemMap["Photo_Note"].InInventory || ItemMap["Photo_Note"].IsUsed) || !ItemMap["Gear"].InInventory
                || !ItemMap["GasMask"].InInventory))
                {
                    return false;
                }
                else
                {
                    return true;
                }
    }

    
    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }


    public void PrintProgress()
    {
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


    public void Ending()
    {
        StartCoroutine(EndLoadTitle());
    }

    public void ToTitleDone()
    {
        ToTitle = true;
    }

    IEnumerator EndLoadTitle()
    {
        yield return null;
        string nextScene = "";

        #if UNITY_EDITOR
            string path = "Assets/Prefabs/LoadingCanvas.prefab";
            GameObject LoadingCanvas = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        #else
            string path = "CustomSprites/LoadingCanvas";
            GameObject LoadingCanvas = Resources.Load(path) as GameObject;
        #endif


        nextScene = "TitleScene";
        yield return new WaitForSeconds(3f);


        GameObject Loading = Instantiate(LoadingCanvas);
        Loading.SetActive(true);
        Loading.GetComponent<Canvas>().enabled = true;

        GameObject Credit = FindObjectOfType<RawImage>(true).gameObject;
        if (Credit) Credit.SetActive(true);

        Button ToTitleButton = FindObjectOfType<Button>(true);
        ToTitleButton.gameObject.SetActive(true);
        
        ToTitleButton.onClick.AddListener(ToTitleDone);
        

        TextMeshProUGUI LoadingText = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        LoadingText.text = "Loading...";



        UnityEngine.AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float minimumLoadingTime = 5.0f;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;


            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {

            }
            else if (ToTitle)
            {
                op.allowSceneActivation = true;
            }
        }

        if (timer < minimumLoadingTime)
        {
            yield return new WaitForSeconds(minimumLoadingTime - timer);
        }

        
        Destroy(Loading);

    }

    void Awake()
    {
        string TempName = "";
        if (SceneManager.GetActiveScene().name == "2hBuildTest2")
        {
            Scenes = EScenes.Room1;
        }
        else if (SceneManager.GetActiveScene().name == "2hRoom2Backup")
        {
            Scenes = EScenes.Room2;
        }
        else
        {
            Scenes = EScenes.Title;
        }


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
        DialogueMap.Add("VentNotDone", false);
        DialogueMap.Add("BirdNotKilled", false);
        DialogueMap.Add("ChemicalsNotStarted", false);
        DialogueMap.Add("NicknameRevealed", false);
        DialogueMap.Add("FInalPuzzleDone", false);

    }

}

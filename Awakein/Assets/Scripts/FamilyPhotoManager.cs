using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamilyPhotoManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public InvenManager invenManager;
    public DialogueManager dialogue;
    public GameFlowManager gameFlowManager;
    public PhotoDragHandler photoDragHandler;
    public Canvas canvas;
    string Ret = "";
    //int n=-1;
   // private bool IsDone = true;
    private bool IsFirst = false;
    private bool hasTriggeredUpdatePuzzle = false; // 추가된 변수

    void Start()
    {
        //gameFlowManager.DialogueMap.Add("SecondWindowDone", false);
        
    }

    public void StartPuzzle()
    {
        //canvas.gameObject.SetActive(true);
        //Debug.Log("하고 있니");

        
         if (IsFirst  && 
                (invenManager.ItemMap["FamilyPhoto"].IsUsed || !invenManager.ItemMap["FamilyPhoto"].InInventory))
        {
            Ret = "The sunlight is already beating down strongly!";
            dialogue.CallRoutine(Ret);
            canvas.gameObject.SetActive(true);
        }
        else if (!IsFirst)
        {
        //    Ret="The sunlight is already shining down intensely. I think there was a similar phrase in a note from the previous room...";
          //  dialogue.CallRoutine(Ret);

            dialogue.QDialogue.Enqueue("The sunlight is already shining down intensely.");
            Ret="I think there was a similar phrase in a note from the previous room...";
            dialogue.CallRoutine(Ret);
            IsFirst=true;
            canvas.gameObject.SetActive(true);
            StartCoroutine(CheckNicknameActive());
        }
        canvas.gameObject.SetActive(true);
        
    }

    public void ExitPuzzle()
    {
        canvas.gameObject.SetActive(false);
    }

    public void TriggerDialogue()
    {
        if (photoDragHandler.Nickname.gameObject.activeSelf == true)
        { 
            invenManager.ItemMap["FamilyPhoto"].IsUsed = true;
            invenManager.RemoveItem("FamilyPhoto");
            gameFlowManager.DialogueMap.Add("SecondWindowing", false);
          //  IsDone = false;
            Ret = "FOREST CHILD LIKE COYOTE...\nI created my dad's nickname using the Air Force voice phonetic alphabet.";


            dialogue.CallRoutine(Ret);
           gameFlowManager.DialogueMap["SecondWindowing"] = true;

           
            //IsDone = true;
            hasTriggeredUpdatePuzzle = true; 
       }
    }

    private IEnumerator CheckNicknameActive()
    {
        while (canvas.gameObject.activeSelf && !hasTriggeredUpdatePuzzle)
        {
            if (photoDragHandler.Nickname.gameObject.activeSelf)
            {
                TriggerDialogue();
                hasTriggeredUpdatePuzzle = true; 
                yield break; 
            }
            yield return new WaitForSeconds(0.5f); 
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dText;
    private TextMeshProUGUI TargetText;
    public GameObject Panel;
    public Button NextBT;
    private Vector3 UIPosition;
    public bool IsDialoguePlaying { get; set; }

    private void Awake()
    {
        this.gameObject.SetActive(true);
        UIPosition = transform.position;
        Panel.SetActive(false);
        
    }
    public void SimpleDialogue(string dialogue)
    {
        Panel.SetActive(true);
        dText.text = dialogue;
    }
    public void WindowDialogue()
    {
        Debug.Log("Window Clicked");
        Panel.SetActive(true);
        PlayDialogue("That's a Window");
    }
    public void DoorDialogue()
    {
        Debug.Log("Door Clicked");
        Panel.SetActive(true);
        PlayDialogue("That's a Door");
    }
    public void CloseDialogue()
    {
        Panel.SetActive(false);
    }

    public void NextButton()
    {
        if (IsDialoguePlaying)
        {
            StopDialogue = true;
        }
        else
        {
            Panel.SetActive(false);
        }
    }
    private bool StopDialogue = false;

    public IEnumerator PlayDialogue(string dialogue)
    {
        Debug.Log("PlayDialogue");
        TargetText = dText;
        TargetText.text = dialogue;
        IsDialoguePlaying = true;
        Panel.SetActive(true);
        dText.text = "";
        for (int i = 0; i < dialogue.Length; i++)
        {
            dText.text += dialogue[i];
            if (StopDialogue)
            {
                dText.text = dialogue;
                StopDialogue = false;
                break;
            }

            if (dialogue[i] == '.' || dialogue[i] == '?' || dialogue[i] == '!' || dialogue[i] == ' ')
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
            
        }
        StopDialogue = false;
        IsDialoguePlaying = false;
    }

    public void CallRoutine(string dialogue)
    {
        StartCoroutine(PlayDialogue(dialogue));
    }
    // public IEnumerator PlayDialogue(string dialogue, GameObject target)
    // {
    //     target.gameObject.SetActive(true);
    //     target.TryGetComponent<Canvas>(out Canvas canvas);
    //     if (canvas != null)
    //     {
    //         canvas.enabled = false;
    //     }

    //     Debug.Log("PlayDialogue");
    //     TargetText = dText;
    //     TargetText.text = dialogue;
    //     IsDialoguePlaying = true;
    //     Panel.SetActive(true);
    //     dText.text = "";
    //     for (int i = 0; i < dialogue.Length; i++)
    //     {
    //         dText.text += dialogue[i];
    //         if (StopDialogue)
    //         {
    //             target.gameObject.SetActive(false);
    //             dText.text = dialogue;
    //             StopDialogue = false;
    //             IsDialoguePlaying = false;
    //             break;
    //         }

    //         if (dialogue[i] == '.' || dialogue[i] == '?' || dialogue[i] == '!' || dialogue[i] == ' ')
    //         {
    //             yield return new WaitForSeconds(0.1f);
    //         }
    //         else
    //         {
    //             yield return new WaitForSeconds(0.05f);
    //         }
            
    //     }
    //     target.gameObject.SetActive(false);
    //     StopDialogue = false;
    //     IsDialoguePlaying = false;
    // }
}

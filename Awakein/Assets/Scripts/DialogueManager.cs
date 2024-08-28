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
    public Queue<string> QDialogue = new Queue<string>();

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
        else if (QDialogue.Count > 0)
        {
            StartCoroutine(PlayDialogue(QDialogue.Peek()));
        }
        else
        {
            Panel.SetActive(false);
        }
    }
    private bool StopDialogue = false;

    public IEnumerator PlayDialogue(string dialogue)
    {
        if (QDialogue.Count == 0) QDialogue.Enqueue(dialogue);
        else if (QDialogue.Peek() != dialogue) QDialogue.Enqueue(dialogue);
        Debug.Log("now on peak: " + QDialogue.Peek());
        
        dialogue = QDialogue.Peek();
        TargetText = dText;
        TargetText.text = dialogue;
        yield return new WaitForSeconds(0.1f);
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
        QDialogue.Dequeue();
        if (QDialogue.Count > 0)
        {
            PlayDialogue(QDialogue.Peek());
        }

        StopDialogue = false;
        IsDialoguePlaying = false;
    }
    IEnumerator WaitAWhile(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void CallRoutine(string dialogue)
    {
        StartCoroutine(PlayDialogue(dialogue));
    }
}
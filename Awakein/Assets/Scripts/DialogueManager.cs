using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dText;
    public GameObject Panel;
    private Vector3 UIPosition;
    private void Awake()
    {
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
        dText.text = "That's a Window";
    }
    public void DoorDialogue()
    {
        Debug.Log("Door Clicked");
        Panel.SetActive(true);
        dText.text = "That's a Door";
    }
    public void CloseDialogue()
    {
        Panel.SetActive(false);
    }
}

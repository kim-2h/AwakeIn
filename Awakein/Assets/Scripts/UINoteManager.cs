using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UINoteManager : MonoBehaviour
{
    public Canvas NoteCanvas, PauseCanvas, ContentCanvas;
    public GameObject[] Books;
    public Button[] BookButtons;
    public GameFlowManager GameFlow;
    public List<GameObject> Contents;
    public Dictionary<string, GameObject> ContentsMap = new Dictionary<string, GameObject>();

    public int SceneNum = 1;

    public void ShowBooks()
    {
        PauseCanvas.enabled = false;
        NoteCanvas.gameObject.SetActive(true);
        NoteCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
    }
    
    public void CloseBooks()
    {
        NoteCanvas.gameObject.SetActive(false);
        NoteCanvas.enabled = false;
        
        Books[0].SetActive(false);
        Books[1].SetActive(false);
        Books[2].SetActive(false);
    }

    public void CloseNote()
    {
        Books[0].SetActive(false);
        Books[1].SetActive(false);
        Books[2].SetActive(false);
    }

    public void BookClicked()
    {
        string Name = EventSystem.current.currentSelectedGameObject.name;
        NoteCanvas.enabled = false;
        Books[Name[4]- '1'].SetActive(true);
        UpdateBook(Name[4] - '1');

    }
    public void DirectBook()
    {
        Books[SceneNum -1].SetActive(true);
        UpdateBook(SceneNum - 1);
    }
    private GameObject NowContent = null;
    private void UpdateBook(int Idx)
    {
        foreach (Transform content in Books[Idx].transform)
        {
            if (GameFlow.ItemMap.ContainsKey(content.name) && (GameFlow.ItemMap[content.name].InInventory ||
                GameFlow.ItemMap[content.name].IsUsed))
            {
                content.gameObject.SetActive(true);
                content.GetComponent<Button>().onClick.AddListener(() => ClickContent(content.name));
            }
        }
    }

    public void ClickContent(string Name)
    {
        if (ContentsMap.ContainsKey(Name))
        {
            ContentsMap[Name].SetActive(true);
            NowContent = ContentsMap[Name];
            ContentCanvas.enabled = true;
            ContentCanvas.gameObject.SetActive(true);
        }

    }
    public void CloseContent()
    {
        if (NowContent != null)
        {
            NowContent.SetActive(false);
            ContentCanvas.gameObject.SetActive(false);
            ContentCanvas.enabled = false;
            NowContent = null;
        }
    }

    void Start()
    {
        this.gameObject.SetActive(true);
        DontDestroyOnLoad(this.gameObject);

        Books[0].SetActive(false);
        Books[1].SetActive(false);
        Books[2].SetActive(false);
        BookButtons[0].interactable = true;
        BookButtons[1].interactable = false;
        BookButtons[2].interactable = false;

        NoteCanvas.gameObject.SetActive(true);
        NoteCanvas.enabled = false;

        foreach (GameObject content in Contents)
        {
            if (content.name.Contains("Note") || content.name.Contains("BookCard")) 
            {
                ContentsMap.Add(content.name, content);
                Debug.Log(content.name + " Added in CMap"); 
            }
               
        }

    }

    void Awake()
    {
        this.gameObject.SetActive(true);
        NoteCanvas.enabled = false;

        BookButtons[0].onClick.AddListener(() => BookClicked());
        BookButtons[1].onClick.AddListener(() => BookClicked());
        BookButtons[2].onClick.AddListener(() => BookClicked());
    }

}

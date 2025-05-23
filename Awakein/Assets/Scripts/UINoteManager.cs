using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        Time.timeScale = 0;
    }
    
    public void CloseBooks()
    {
        NoteCanvas.gameObject.SetActive(false);
        NoteCanvas.enabled = false;
        
        Books[0].SetActive(false);
        Books[1].SetActive(false);
        Books[2].SetActive(false);

        Time.timeScale = 1;
    }

    public void CloseNote()
    {
        Books[0].SetActive(false);
        Books[1].SetActive(false);
        Books[2].SetActive(false);

        Time.timeScale = 1;
    }

    public void BookClicked()
    {
        string Name = EventSystem.current.currentSelectedGameObject.name;
        NoteCanvas.enabled = false;
        Books[Name[4]- '1'].SetActive(true);
        UpdateBook(Name[4] - '1');

    }

    public void Book2Clicked()
    {
        NoteCanvas.enabled = false;
        Books[1].SetActive(true);
        UpdateBook(1);
    }

    public void Book1Clicked()
    {
        NoteCanvas.enabled = false;
        Books[0].SetActive(true);
        UpdateBook(0);
    }

    public void DirectBook()
    {
        Time.timeScale = 0;
        Books[0].SetActive(true);
        UpdateBook(0);
    }

    public void DirectBook2()
    {
        Time.timeScale = 0;
        Books[1].SetActive(true);
        UpdateBook(1);
    }
    private GameObject NowContent = null;
    private void UpdateBook(int Idx)
    {
        foreach (Transform content in Books[Idx].transform)
        {
            if (content.name.Contains("Note") || content.name.Contains("BookCard")) 
            {
                content.GetComponent<Button>().onClick.AddListener(() => ClickContent(content.name));
            }

            if (GameFlow.ItemMap.ContainsKey(content.name) && (GameFlow.ItemMap[content.name].InInventory ||
                GameFlow.ItemMap[content.name].IsUsed))
            {
                content.gameObject.SetActive(true);
                
            }
        }
    }

    public void ClickContent(string Name)
    {
        Debug.Log("Clicked note content: " + Name);
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
        // GameObject[] NNotes = GameObject.FindGameObjectsWithTag("NoteCanvas");
        // if (NNotes.Length >1) Destroy(this.gameObject);
        // else DontDestroyOnLoad(this.gameObject);

        Books[0].SetActive(false);
        Books[1].SetActive(false);
        Books[2].SetActive(false);
        BookButtons[0].interactable = true;
        BookButtons[1].interactable = false;
        BookButtons[2].interactable = false;
        
        if (SceneManager.GetActiveScene().name == "2hRoom2Backup") SceneNum = 2;
        if (SceneNum == 2) BookButtons[1].interactable = true;

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
        //BookButtons[2].onClick.AddListener(() => BookClicked());
    }

}

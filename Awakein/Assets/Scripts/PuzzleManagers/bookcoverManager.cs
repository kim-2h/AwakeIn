using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCoverManager: MonoBehaviour,IPuzzle
{
    //북 터치하면 canvas 열고 열리면 시작하기 
   
   [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    private Vector3 CameraPosition;
    //public GameObject secondBookShelf;
    public InvenManager invenManager;
    public void StartPuzzle()
    {
        if (invenManager.ItemMap["Carpet_Note"].IsUsed){
            Debug.Log("BookShelf Puzzle Started");
            canvas.gameObject.SetActive(true);
        }
        else return;
       
    }
    public void ExitPuzzle()
    {
        Debug.Log("BookShelf Puzzle Exit");
        canvas.gameObject.SetActive(false);
        //secondBookShelf.GetComponent<SecondBookShelf>().BookShelfFinished();
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    void Start()
    {
        CameraPosition = Camera.main.gameObject.transform.position;
    }

   
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBookShelfManager : MonoBehaviour, IPuzzle
{
   
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    private Vector3 CameraPosition;
    
    //public GameObject secondBookShelf;

    public void StartPuzzle()
    {
        Debug.Log("BookShelf Puzzle Started");
        canvas.gameObject.SetActive(true);
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

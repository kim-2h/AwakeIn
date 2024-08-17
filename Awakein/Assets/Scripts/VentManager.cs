using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class VentManager : MonoBehaviour,IPuzzle
{
  [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    private Vector3 CameraPosition;
   public GameObject Over;
     VentDragHandler ventDragHandler;
     public GameFlowManager gameflow;
     public GameObject Driver;
    
    //public bool[4] Each=false;
    public void StartPuzzle()
    {
        Debug.Log("Vent Puzzle Started");
        canvas.gameObject.SetActive(true);
        
        if (gameflow.ItemList[20].InInventory){
          Driver.SetActive(true);
        }
        else Driver.SetActive(false);
    }
    void Start()
    {
        CameraPosition = Camera.main.gameObject.transform.position;
    }
    public void ExitPuzzle()
    {
      
     ventDragHandler = FindObjectOfType<VentDragHandler>(); 
        //Debug.Log(ventDragHandler.CoroutineStarted);
                

       if (ventDragHandler&&!ventDragHandler.CoroutineStarted||Driver.activeSelf==false) canvas.gameObject.SetActive(false);
         else  Debug.Log("Wait");
        //secondBookShelf.GetComponent<SecondBookShelf>().BookShelfFinished();
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    
   /* public void OnClick(){
      if (!ventDragHandler.CoroutineStarted) ExitPuzzle();
      else  Debug.Log("Wait");
    }*/
    
}

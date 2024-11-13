using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class VentManager : MonoBehaviour, IPuzzle
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
        
        //(gameflow.ItemList[20].InInventory) <<죄송한데 이러시면 안되는게 리스트는 위치바뀔수도 있고요 
        //제가 쓰레기아이템 삭제해서 오류났는데 뭔가햇네 제가 딕셔너리로 바꿔둠
        if  (gameflow.ItemMap["Driver"].InInventory) 
        {
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
                

       if (ventDragHandler&&!ventDragHandler.CoroutineStarted) canvas.gameObject.SetActive(false);
         else  Debug.Log("Wait");
        //secondBookShelf.GetComponent<SecondBookShelf>().BookShelfFinished();
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    
   /* public void OnClick(){
      if (!ventDragHandler.CoroutineStarted) ExitPuzzle();
      else  Debug.Log("Wait");
    }*/
    
}

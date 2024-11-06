using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.EventSystems;
public class SignalBookManager : MonoBehaviour
{
   public InvenManager invenManager;
    private RaycastHit hit;
   public Canvas canvas;
   private Vector3 CameraPosition;
    void OnMouseDown(){
      canvas.gameObject.SetActive(true);
    }
    
    public void ExitPuzzle()
    {
        Debug.Log("BookShelf Puzzle Exit");
        canvas.gameObject.SetActive(false);
      
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    //사라져야함 
    
}

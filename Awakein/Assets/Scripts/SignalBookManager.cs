using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.EventSystems;
public class SignalBookManager : MonoBehaviour, IPuzzle
{
   public InvenManager invenManager;
    private RaycastHit hit;
   public Canvas canvas, Canvas2;
   private Vector3 CameraPosition;
   public GameObject BookFront, BookBack;

    [SerializeField] public bool IsSolved { get; set; }
    public void StartPuzzle(){

      Debug.Log("Book map Puzzle Started : " + this.name);
      if (this.name == "SignalBook")
      {
        canvas.gameObject.SetActive(true);
        BookBack.SetActive(false);
        BookFront.SetActive(true);
      }
      else if (this.name == "WorldMap")
      {
        Canvas2.gameObject.SetActive(true);
      }

    }
    
    public void ExitPuzzle()
    {
        Debug.Log("BookShelf Puzzle Exit");
        canvas.gameObject.SetActive(false);
      
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    public void ExitPuzzle2()
    {
        Debug.Log("WorldMap Puzzle Exit");
        Canvas2.gameObject.SetActive(false);
      
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    public void BookClicked()
    {
      SoundManager.Instance.PlaySFX(13);
        BookFront.SetActive(!BookFront.activeSelf);
        BookBack.SetActive(!BookBack.activeSelf);
    }

    void Start()
    {
        CameraPosition = Camera.main.gameObject.transform.position;
        canvas.gameObject.SetActive(false);
    }
    //사라져야함 
    
}

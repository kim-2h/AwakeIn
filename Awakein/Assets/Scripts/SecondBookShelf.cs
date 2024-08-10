using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondBookShelf : MonoBehaviour
{
     [SerializeField]
     private GameObject[] Slots;
     public GameObject[] Books;
     public GameObject bookShelfManager;
     private bool isSolved = false;
     moving movingbooks;
  
     slot slot;
      void Start(){
        Slots = GameObject.FindGameObjectsWithTag("2ndBookShelf");
        Books = GameObject.FindGameObjectsWithTag("Book");
        //bookShelfManager=GetComponent<BookShelfManager>();
     }
     
     void Update(){
      BookShelfFinished();
     }
     public void BookShelfFinished(){
      /*  if (Slots[0].GetComponent<slot>().Books()==null) {
           Debug.Log("dsf");
        }*/
        if (Slots[0].GetComponent<slot>().Books()==Books[2]&&
        Slots[1].GetComponent<slot>().Books()==Books[0]&&
        Slots[2].GetComponent<slot>().Books()==Books[4]&&
        Slots[3].GetComponent<slot>().Books()==Books[5]&&
        Slots[4].GetComponent<slot>().Books()==Books[1]&&
        Slots[5].GetComponent<slot>().Books()==Books[3]){
         foreach(GameObject item in Books){
             item.GetComponent<Image>().raycastTarget = false;
         }
          bookShelfManager.gameObject.GetComponent<IPuzzle>().IsSolved = true;
           //Debug.Log("Done");
         
        }
     }
}

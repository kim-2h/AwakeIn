using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class slot : MonoBehaviour,IDropHandler
{
    public GameObject Books(){
       if(transform.childCount>0){
        return transform.GetChild(0).gameObject;
       }
       else return null;
    }
    public void OnDrop(PointerEventData eventData){
        if (Books()==null){
            moving.beingDraggedIcon.transform.SetParent(transform);
            Debug.Log(gameObject.name+gameObject.transform.GetChild(0).name);
            moving.beingDraggedIcon.transform.position=transform.position;
        }
        
    }
    
}

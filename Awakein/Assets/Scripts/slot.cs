using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class slot : MonoBehaviour,IDropHandler
{
    GameObject Books(){
       if(transform.childCount>0){
        return transform.GetChild(0).gameObject;
       }
       else return null;
    }
    public void OnDrop(PointerEventData eventData){
        if (Books()==null){
            moving.beingDraggedIcon.transform.SetParent(transform);
            moving.beingDraggedIcon.transform.position=transform.position;
        }
        else {
             if (moving.beingDraggedIcon.transform.position.x<=transform.GetChild(0).gameObject.postion.x+0.1&&moving.beingDraggedIcon.transform.position.x>=transform.GetChild(0).gameObject.postion.x-0.1)
             {transform.GetChild(0).gameObject
                
             }
        }
    }
    
}

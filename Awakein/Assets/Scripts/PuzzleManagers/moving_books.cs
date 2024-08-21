using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class moving : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{   /*1.책을 마우스로 집음*/
public static GameObject beingDraggedIcon;
Vector3 startPosition;
[SerializeField] public Transform onDragParent;
[HideInInspector] public Transform startParent;

  
public void OnBeginDrag(PointerEventData eventData){
    beingDraggedIcon=gameObject;
    //startPosition=transform.position;
    startPosition = GetComponent<RectTransform>().anchoredPosition;
    //GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    Debug.Log("start pos " + startPosition);
    startParent=transform.parent;

    GetComponent<CanvasGroup>().blocksRaycasts=false;
    transform.SetParent(onDragParent);
}
public void OnDrag(PointerEventData eventData){
    transform.position=Input.mousePosition;
}
public void OnEndDrag(PointerEventData eventData){
    beingDraggedIcon=null;
    GetComponent<CanvasGroup>().blocksRaycasts=true;
    if(transform.parent==onDragParent){
        transform.SetParent(startParent);
        //transform.position=startPosition;
        //GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        GetComponent<RectTransform>().anchoredPosition = startPosition;
    }
    GetComponent<RectTransform>().anchoredPosition = startPosition;
}

}
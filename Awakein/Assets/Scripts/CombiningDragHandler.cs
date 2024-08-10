using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombiningDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject[] MatchOtherParts;
    public CombiningManager CombiningManager => GameObject.Find("InvenCanvas").transform.
    Find("InvenBG").GetComponent<CombiningManager>();
    GameObject MatchingObject;
    public bool isMatched = false;
    public bool[] bools;
    // Called when the drag operation starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (isMatched) return;
        foreach (GameObject part in MatchOtherParts)
        {
            
            if (part.name == gameObject.name+" (1)")
            {
               // Debug.Log("Match found: ");
                MatchingObject=part;
                Debug.Log(MatchingObject.name);
            }
        }
    }
    
    // Called while dragging
    public void OnDrag(PointerEventData eventData)
    {
        if (isMatched) return;
        transform.position=eventData.position;
       
    }
    
    // Called when the drag operation ends
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isMatched) return;
       if (transform.position.x<=MatchingObject.transform.position.x+15&&
       transform.position.x>=MatchingObject.transform.position.x-15&&
       transform.position.y<=MatchingObject.transform.position.y+15&&
       transform.position.y>=MatchingObject.transform.position.y-15)
    {
     
        transform.position=MatchingObject.transform.position;
        isMatched=true;
        CombiningManager.InvokeMatch();
    }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Image = UnityEngine.UI.Image;
public class GearDragHandler : MonoBehaviour,IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    public GameObject GearPlace;
    public InvenManager invenManager;
    public static Vector3 DefaultPos;
    public bool IsPlaced=false;
    public RectTransform rectTransform;
    public TrainManager trainManager;
    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    { 
        if (IsPlaced) return;
        transform.position=eventData.position;
        Debug.Log("Moiving");
    }
    public void OnDrag(PointerEventData eventData){
      if (IsPlaced) return;
       transform.position=eventData.position;
        rectTransform.localScale = new Vector3(1f, 1.2f, 1.5f);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (IsPlaced) return;

        if (Vector3.Distance(transform.position,GearPlace.GetComponent<RectTransform>().position)<100){
            transform.position=GearPlace.transform.position;
            Debug.Log("Thatsit");
            IsPlaced=true;
            invenManager.ItemMap["Gear"].IsUsed=true;
            invenManager.RemoveItem("Gear");
            SoundManager.Instance.PlaySFX(4);
            this.GetComponent<Image>().raycastTarget=false;
            rectTransform.localScale = new Vector3(1f, 1.2f, 1.5f);

            if (invenManager.ItemMap["Train_Note"].InInventory || invenManager.ItemMap["Train_Note"].IsUsed) 
            trainManager.SetupNote();
        }
        else 
        {
            transform.position=DefaultPos;
            rectTransform.localScale = new Vector3(2f, 2.4f, 1.5f);
        }
        }


    // Update is called once per frame
    void Start()
    {
        DefaultPos=transform.position;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(2f, 2.4f, 1.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoDragHandler : MonoBehaviour,IBeginDragHandler ,IDragHandler, IEndDragHandler
{
    public GameObject PhotoPlace;
    public InvenManager invenManager;
    public static Vector3 DefaultPos;
    public bool IsPlaced=false;
     public Text Nickname;
    public GameObject Father;
     private float fadeSpeed=0.3f;

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
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (IsPlaced) return;

        if (Vector3.Distance(transform.position,PhotoPlace.GetComponent<RectTransform>().position)<100){
            transform.position=PhotoPlace.transform.position;
            IsPlaced=true;
           StartCoroutine(FadeOut());
           
            //Debug.Log("히히 포토 나타났지롱");
             invenManager.ItemMap["FamilyPhoto"].IsUsed = true;
            invenManager.RemoveItem("FamilyPhoto");
        }
        else transform.position=DefaultPos;
        }


   
    void Start()
    {
        DefaultPos=transform.position;
             Nickname.gameObject.SetActive(false);

    }
    IEnumerator FadeIn(){//이거 text로 할건지 이미지로 할건지 내일 회의가서 물어봐야함
       // Debug.Log("하고 있음?");
        Color newColor = Nickname.color;           
        newColor.a = 0f;
       while(newColor.a<1f){
          newColor.a+=Time.deltaTime*fadeSpeed*2f;
         Nickname.color = newColor;
         yield return null;
       }
     Nickname.gameObject.SetActive(true);
    }
   IEnumerator FadeOut(){
        //Debug.Log("하고 있음?");
        Color newColor = Father.GetComponent<Image>().color;           
        newColor.a = 1f;
       while(newColor.a>0.65f){
          newColor.a-=Time.deltaTime*fadeSpeed;
         Father.GetComponent<Image>().color = newColor;
         yield return null;
       }
       StartCoroutine(FadeIn());
     //Nickname.gameObject.SetActive(true);
    }
   
}

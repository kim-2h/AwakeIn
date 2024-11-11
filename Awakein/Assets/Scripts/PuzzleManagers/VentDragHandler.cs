using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class VentDragHandler : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
   public bool isCleared = false;
   private int BoltsDone=0;
   private float timer=2f;
   public bool CoroutineStarted=false;
   public float fadeSpeed = 0.5f;
   public VentManager ventManager;
   public InvenManager invenManager;
   public GameObject Vent;
   public GameObject[] Bolts;
   Vector3 beginposition;
   void Start()
   {
    
    //invenManager=GetComponent<InvenManager>();
    Bolts=GameObject.FindGameObjectsWithTag("Bolt");
     beginposition=transform.position;
   }
    public void OnBeginDrag(PointerEventData eventData)
    { 
        if (isCleared) return;
         transform.position=eventData.position;
    }
    public void OnDrag(PointerEventData eventData){
      if (isCleared) return;
       transform.position=eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        if (isCleared) return;
        var Pos = GetComponent<RectTransform>().anchoredPosition;
        

        // for (int i=0;i<4;i++){
        // if (transform.position.x <= Bolts[i].transform.position.x+50&&
        // transform.position.x >= Bolts[i].transform.position.x-50&&
        // transform.position.y <= Bolts[i].transform.position.y+50&&
        // transform.position.y >= Bolts[i].transform.position.y-50)
        // {
        //    StartCoroutine(BoltRotating(Bolts[i]));
        // }
        
        for (int i = 0; i<4; i++)
        {
          var Dist = Vector2.Distance(Pos, Bolts[i].GetComponent<RectTransform>().anchoredPosition);
          Debug.Log("Distant : " + Dist + " pos : " + Pos + " Bolt pos : " + Bolts[i].GetComponent<RectTransform>().anchoredPosition);
            if (Dist < 100f)
            {
                SoundManager.Instance.PlaySFX(13);
                StartCoroutine(BoltRotating(Bolts[i]));
            }
          if (BoltsDone == 4){
            SoundManager.Instance.PlaySFX(2);
             StartCoroutine(MovingVent(Vent));
             transform.position = beginposition;
              
             invenManager.ItemMap["Driver"].IsUsed = true;
             invenManager.RemoveItem("Driver");
            isCleared=true;
            ventManager.IsSolved = true;
         } 
         else
         {
             transform.position = beginposition;
         }
         
      }
    }
   
    IEnumerator BoltRotating(GameObject tf){
     
      if (CoroutineStarted) yield break;
       BoltsDone++;
      CoroutineStarted=true;
      //Debug.Log(CoroutineStarted);
        float elapsedTime = 0f;
       while (elapsedTime < timer)
        {
            tf.transform.Rotate(new Vector3(0f, 0f,30f) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeOut(tf));
        
         Debug.Log(BoltsDone);
    }
    IEnumerator FadeOut(GameObject tf){
       Image image = tf.GetComponent<Image>(); 
      Color newColor = image.color;           
        newColor.a = 1f;
       while(newColor.a>0){
          newColor.a-=Time.deltaTime*fadeSpeed;
          image.color=newColor;
         yield return null;
       }
     tf.SetActive(false);
     
     CoroutineStarted=false;
      Debug.Log(CoroutineStarted);
    }
    IEnumerator MovingVent(GameObject tf){
       yield return new WaitForSeconds(3f);
      CoroutineStarted=true;
      
      Vector3 startPosition = tf.transform.position;
        Vector3 endPosition = new Vector3(tf.transform.position.x+50 , tf.transform.position.y-50, tf.transform.position.z);
        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            tf.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / timer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tf.transform.position = endPosition;
         StartCoroutine(FadeOut(tf));
        
    }

   
    }


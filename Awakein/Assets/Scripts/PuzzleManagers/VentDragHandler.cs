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
   public float fadeSpeed = 10f;
   public VentManager ventManager;
   public InvenManager invenManager;
   public GameObject Vent;
   public GameObject[] Bolts;
   public DialogueManager Dialogue;
   public UISetting UISetting;

   public GameFlowManager GameFlow;
    public Dictionary<string, IPuzzle> PuzzleMap = new Dictionary<string, IPuzzle>();
    public Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();

   Vector3 beginposition;
   void Start()
   {
    
    //invenManager=GetComponent<InvenManager>();
    Bolts=GameObject.FindGameObjectsWithTag("Bolt");
     beginposition=transform.position;
     PuzzleMap = GameFlow.PuzzleMap;
     ItemMap = invenManager.ItemMap;
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
        
       foreach (GameObject bo in Bolts)
        {
          var Dist = Vector2.Distance(Pos, bo.GetComponent<RectTransform>().anchoredPosition);
          //Debug.Log("Distant : " + Dist + " pos : " + Pos + " Bolt pos : " + Bolts[i].GetComponent<RectTransform>().anchoredPosition);
            if ( Dist< 150f)
            {
                StartCoroutine(BoltRotating(bo));
            }
          if (BoltsDone == 4){
            
            
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
      CoroutineStarted=true;
      //if (CoroutineStarted) yield break;
       BoltsDone++;
      //CoroutineStarted=true;
      SoundManager.Instance.PlaySFX(13);
        float elapsedTime = 0f;
       while (elapsedTime < timer)
        {
            tf.transform.Rotate(new Vector3(0f, 0f,30f) * Time.deltaTime*fadeSpeed);
            elapsedTime += Time.deltaTime;
            CoroutineStarted=true;
            yield return null;
        }
        StartCoroutine(FadeOut(tf));
         Debug.Log(BoltsDone);
    
    }
    IEnumerator FadeOut(GameObject tf){
    
       Image image = tf.GetComponent<Image>(); 
      Color newColor = image.color;           
        newColor.a = 1f;
       while(newColor.a>0.01f){
          newColor.a-=Time.deltaTime*fadeSpeed;
          image.color=newColor;
          CoroutineStarted=true;
         yield return null;
       }
     tf.SetActive(false);
      Debug.Log(CoroutineStarted);
      CoroutineStarted=false;
    }
    IEnumerator MovingVent(GameObject tf){
      CoroutineStarted=true;
       yield return new WaitForSeconds(3f);
      
      
      Vector3 startPosition = tf.transform.position;
        Vector3 endPosition = new Vector3(tf.transform.position.x+50 , tf.transform.position.y-50, tf.transform.position.z);
        float elapsedTime = 0f;
       SoundManager.Instance.PlaySFX(2);
        while (elapsedTime < timer)
        {
          CoroutineStarted=true;
            tf.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / timer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tf.transform.position = endPosition;
        
         foreach (Transform holes in tf.GetComponentsInChildren<Transform>()){
           StartCoroutine(FadeOut(holes.gameObject));
         }

        
    }

    public void ClearPuzzle()
    {
        Bolts[0].SetActive(false);
        Bolts[1].SetActive(false);
        Bolts[2].SetActive(false);
        Bolts[3].SetActive(false);

        Vent.SetActive(false);
    }

  public void MoveRoom()
  {

      if (!GameFlow.Room2Test())
      {
          string text = "I can't leave yet.\nI think I need to look around more.";
          Dialogue.CallRoutine(text);
      }
      else if (GameFlow.Room2Test())
      {
          string text = "I can get out through the vent!";
          Dialogue.CallRoutine(text);
          UISetting.ToRoom2();
      }
  }

   
    }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TrainManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public GameObject Train_Note;
    public static Vector3 DefaultPos;
    public Canvas canvas1;
    public Canvas canvas2;
    public DialogueManager dialogueManager;

    public InvenManager invenManager;
    private Vector3 CameraPosition;
    private float timer = 2f;
    public Slider Timer;
    int Maxvalue = 100;
  
    public GameObject Train;
    private bool PersonIsClicked = false;
    private bool IsTimerRunning = false;
    //public bool bDown=false;
    public RectTransform[] Directions;
    public Button[] Family;
    public Button Citizen;
    public Image Blood;
    public Button Cabinet_Key;
    Vector3 startposition;
    private bool HasChosed = false;
    float totalduration = 2f;
    //private float segmentDuration;
    // Start is called before the first frame update
    void Start()
    {
         Timer.maxValue = Maxvalue;
        startposition=Train.transform.position;
        CameraPosition = Camera.main.gameObject.transform.position;
       
    }
    public void NoteClicked()
    {
       invenManager.ItemAdder("Train_Note");
        if (invenManager.ItemMap["Gear"].IsUsed) SetupNote();
    }
    public void KeyClicked()
    {
       invenManager.ItemAdder("Cabinet_Key");
        IsSolved = true;
        
    }

    public void SetupNote()
    {
           canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);
        Train_Note.SetActive(true);
    }

    public void NoteClosed()
    {
        if (!IsTimerRunning)
        StartCoroutine(Slider());
        Train_Note.SetActive(false);
    }
    
      public void StartPuzzle()
    {
        //Debug.Log("BookShelf Puzzle Started");
       if(!invenManager.ItemMap["Gear"].IsUsed || (!invenManager.ItemMap["Train_Note"].InInventory && 
       !invenManager.ItemMap["Train_Note"].IsUsed)) 
        canvas1.gameObject.SetActive(true);
    
       else {
        canvas2.gameObject.SetActive(true);
        if (!HasChosed) StartCoroutine(Slider());}
    }
     public void ExitPuzzle()
    {
      if (!HasChosed && canvas2.isActiveAndEnabled) return;
        Debug.Log("TrainPuzzle Exit"); Debug.Log(invenManager.ItemMap["Gear"].IsUsed);
      Debug.Log(Cabinet_Key.gameObject.activeSelf);
        
        
        canvas2.gameObject.SetActive(false);
       canvas1.gameObject.SetActive(false);
          
        Camera.main.gameObject.transform.position = CameraPosition;
    }
     public void PersonClicked(int btn){
  if (btn == 0)
  {
    PersonIsClicked=true;
  // Debug.Log(PersonIsClicked);
    return;
  }
  else PersonIsClicked=false;
    }
 IEnumerator Slider(){
   IsTimerRunning=true;
       float Slidertime=20f;
       while (IsTimerRunning&&Timer.value < Maxvalue&&!PersonIsClicked)
        {
            Timer.value += Time.deltaTime*Slidertime;
            if (PersonIsClicked) 
                 {
                     break;
               }
           
            if (Timer.value >= Maxvalue)
            {
                Timer.value = Maxvalue;
                break;
            }
            yield return null;
        }
        StartCoroutine(Choosing());
 }
    IEnumerator Choosing(){
      int currentSegment = 0;
        float segmentDuration = totalduration / (Directions.Length-1);
        

        if  (PersonIsClicked) {
          Timer.value = Maxvalue;
          dialogueManager.Panel.SetActive(true);
          StartCoroutine(dialogueManager.PlayDialogue("BlahBlah"));
         yield return new WaitForSeconds(3);
         while (currentSegment < Directions.Length - 1)
        {
            Vector3 start = Directions[currentSegment].position;
            Vector3 end = Directions[currentSegment + 1].position;
            float timeElapsed = 0f;

            while (timeElapsed < segmentDuration)
            {
                float t = timeElapsed / segmentDuration;
               Train.GetComponent<RectTransform>().position = Vector3.Lerp(start, end, t); 
               foreach (Button s in Family){
                if (Vector3.Distance(s.GetComponent<RectTransform>().position, 
                Train.GetComponent<RectTransform>().position)<20)
                s.gameObject.SetActive(false);
               }
                timeElapsed += Time.deltaTime;
                yield return null;
            }


            Train.GetComponent<RectTransform>().position = end;
            currentSegment++;
        }
        StartCoroutine(Bleeding());
        
       // Train.GetComponent<RectTransform>().position=targetposition;
       IsTimerRunning=false;
       
         yield return null;
       //
    }
       
    //timeElapsed = 0f;
    else if  (Timer.value==Maxvalue&&!PersonIsClicked) {
          dialogueManager.Panel.SetActive(true);
          StartCoroutine(dialogueManager.PlayDialogue("You can get a promotion and even solve crimes, so what" + 
          "is there to worry about?"));
         yield return new WaitForSeconds(3);
          while (currentSegment < Directions.Length - 1)
        {
            Vector3 start =  Directions[currentSegment].position;
            Vector3 end =  Directions[currentSegment + 1].position;
            float timeElapsed = 0f;

            while (timeElapsed < segmentDuration)
            {
                float t = timeElapsed / segmentDuration;
               Train.GetComponent<RectTransform>().position = Vector3.Lerp(start, end, t); 
                timeElapsed += Time.deltaTime;
                foreach (Button s in Family){
                if (Vector3.Distance(s.GetComponent<RectTransform>().position, 
                Train.GetComponent<RectTransform>().position)<20)
                s.gameObject.SetActive(false);
               }
                yield return null;
            }

            // 구간 완료 후 마지막 위치로 정확하게 설정
            Train.transform.position = end;
            currentSegment++; // 다음 구간으로 이동
        }
        
        StartCoroutine(Bleeding());
        
        //rain.GetComponent<RectTransform>().position=targetposition;
         yield return null;
        //IsTimerRunning=false;
    }
}
public void StopButton(){
  if (PersonIsClicked){
    return;
  }
}
public IEnumerator Bleeding(){
      Family[0].gameObject.SetActive(false);
      Family[1].gameObject.SetActive(false);
      Family[2].gameObject.SetActive(false);
  HasChosed=true;
  float fadeSpeed = 0.3f;
  //Image image = Blood.GetComponent<Image>(); 
      Color newColor = Blood.color;           
        newColor.a = 0f;
       while(newColor.a<1){
          newColor.a+=Time.deltaTime*fadeSpeed;
          Blood.color=newColor;

          if (newColor.a >0.6f) Cabinet_Key.gameObject.SetActive(true);

         yield return null;
       }
       Cabinet_Key.gameObject.SetActive(true);
       Debug.Log("Family died");
}
}
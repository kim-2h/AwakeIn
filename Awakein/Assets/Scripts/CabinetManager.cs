using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CabinetManager : MonoBehaviour,IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public RectTransform startShape;
    public RectTransform endShape;
    public Canvas canvas;
    public GameObject[] Solutions;
    private Vector3 CameraPosition;
    private bool IsLocked=true;
    public GameObject Cabinet_Key;
    public InvenManager invenManager;
    public RectTransform Lock;
     void Start()
    {
        CameraPosition = Camera.main.gameObject.transform.position;
        Keypos = new Vector3(606f, -207f, 0f);
        //Cabinet_Key.GetComponent<RectTransform>().position;
    }
     public void StartScaling()
    {
         Debug.Log(invenManager.ItemMap["Cabinet_Key"].IsUsed);
         Debug.Log("Key is used! Cabinet");
        if (invenManager.ItemMap["Cabinet_Key"].IsUsed)
        StartCoroutine(ScaleOverTime(2.0f));
        
    }
public void StartPuzzle()
    {
        Debug.Log("BookShelf Puzzle Started");
        canvas.gameObject.SetActive(true);
        if (invenManager.ItemMap["Cabinet_Key"].InInventory){
            Cabinet_Key.SetActive(true);
        }
        
    }
    public void ExitPuzzle()
    {
        Debug.Log("BookShelf Puzzle Exit");
        canvas.gameObject.SetActive(false);
      
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    
    public void Clicked(){
        //Debug.Log("Its me");
        string Name=EventSystem.current.currentSelectedGameObject.name;
        //Debug.Log(Name);
        invenManager.ItemAdder(Name);
    }
    
    IEnumerator ScaleOverTime(float duration)
    {
       
        float timeElapsed = 0;
        Debug.Log("Is it doing right?");
        Vector2 initialSize = startShape.sizeDelta;
        Vector2 targetSize = endShape.sizeDelta;
        Vector3 initialposition= startShape.position;
        Vector3 endposition= endShape.position;
        while (timeElapsed < duration)
        {
        
            float t = timeElapsed / duration;
            Debug.Log("t"+t);
            startShape.sizeDelta = Vector2.Lerp(initialSize, targetSize, t);
            startShape.position=Vector3.Lerp
            (initialposition, endposition, t);
            timeElapsed += Time.deltaTime;
            Debug.Log(startShape.position);

            yield return null;
        }
        startShape.sizeDelta = targetSize;
        yield return new WaitForSeconds(2f);
       
    }
    private Vector3 Keypos;
    public void Drag(){
      if (!IsLocked) return;
       Cabinet_Key.transform.position=Input.mousePosition;
    }
    public void OnEnd()
    {
        
        if (Vector2.Distance(Cabinet_Key.transform.position,Lock.position)<50)
       {
            Debug.Log("Thatsit");
            IsLocked=false;
            invenManager.ItemMap["Cabinet_Key"].IsUsed=true;
            invenManager.RemoveItem("Cabinet_Key");
            Cabinet_Key.SetActive(false);
            Lock.gameObject.SetActive(false);
            StartScaling();
        }
        else
        {
            Cabinet_Key.transform.position = new Vector3(0, 0, 0);
            Cabinet_Key.GetComponent<RectTransform>().anchoredPosition=Keypos;
        }
        //else transform.position=DefaultPos;
        }

}

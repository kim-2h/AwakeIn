using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TornedDollManager : MonoBehaviour
{
     ImageChange imageChange;
     public List<GameObject> Appearing=new List<GameObject>(); 
     Canvas canvas;
     GameObject arm;
     public GameObject InvenManager;
    // Start is called before the first frame update
    bool ONce=true;
    int n=0;
    void Start()
    {
        imageChange=GetComponent<ImageChange>();
        canvas=gameObject.transform.parent.GetComponent<Canvas>();
    }

    // Update is called once per frame
    
    public void Switch(){
        if (!ONce) return;
        if (ONce){
            imageChange.SwitchSprite(gameObject, "doll");
          arm=Instantiate(Appearing[0],canvas.transform);
         Appearing[1].SetActive(true);
          arm.transform.position=new Vector3(transform.position.x+90,transform.position.y+15,transform.position.z);
           StartCoroutine(Arm_torning());
           ONce=false;
        }
        }

    
    
    public void ClosePopUp()
    {
        canvas.gameObject.SetActive(false);
    }
    
    public void KeyClicked()
    {
        InvenManager.GetComponent<InvenManager>().ItemAdder("RadioBattery");
    }
    public IEnumerator Arm_torning(){
        
        Vector3 startposition=arm.transform.position;
        Vector3 endposition=new Vector3(startposition.x+20,startposition.y-20,startposition.z);
        float elapsedTime=0f;
        float time=2f;
        while(elapsedTime < time){
            arm.transform.position=Vector3.Lerp(startposition,endposition,elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        arm.transform.position=endposition;
        Debug.Log("DOne");
    }
}
    
        
        

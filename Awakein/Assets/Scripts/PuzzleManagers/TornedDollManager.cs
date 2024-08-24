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
     public InvenManager invenManager;
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
        if (ONce)
        {
            imageChange.SwitchSprite(gameObject, "DollBody");
            arm=Instantiate(Appearing[0],canvas.transform);
            Appearing[1].SetActive(true);
            arm.GetComponent<RectTransform>().anchoredPosition =new Vector3(60,-45,0);
            Debug.Log(arm.transform.position);
            arm.GetComponent<Image>().alphaHitTestMinimumThreshold=0.9f;
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
        invenManager.ItemMap["Doll"].IsUsed = true;
        invenManager.RemoveItem("Doll");
        if( invenManager.ItemMap["Doll"].IsUsed)
        Debug.Log("DollDisappeared");
        invenManager.ItemAdder("RadioBattery");
    }
    public IEnumerator Arm_torning(){
        
        Vector3 startposition = arm.transform.position;
        Vector3 endposition=new Vector3(startposition.x+30,startposition.y-20,startposition.z);
        float elapsedTime=0f;
        float time=1.5f;
        while(elapsedTime < time){
            arm.transform.position=Vector3.Lerp(startposition,endposition,elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        arm.transform.position=endposition;
        Debug.Log("DOne");
    }
}
    
        
        

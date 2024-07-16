using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class torneddoll : MonoBehaviour
{
     ImageChange imageChange;
     public List<GameObject> Appearing=new List<GameObject>(); 
     Canvas canvas;
     GameObject arm;
    // Start is called before the first frame update
    bool ONce=false;
    int n=0;
    void Start()
    {
        
        imageChange=GetComponent<ImageChange>();
        canvas=gameObject.transform.parent.GetComponent<Canvas>();
    }

    // Update is called once per frame
    
    public void Switch(){
        if (ONce){
            imageChange.SwitchSprite(gameObject, "doll");
         arm=Instantiate(Appearing[0],canvas.transform);
        Appearing[1].SetActive(true);
       arm.transform.position=new Vector3(transform.position.x+90,transform.position.y+15,transform.position.z);
        }
        }

    public void Torning(){
        if (ONce){
        StartCoroutine(Arm_torning());
         ONce=false;}
        
    }
        
    public void ONCE(){
        n++;
        if (n==1) ONce=true;
       
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
    
        
        

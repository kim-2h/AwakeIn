using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookcoverMoving : MonoBehaviour
{
    bool Once = false;
    public GameObject bookinside;
    [SerializeField]
    GameObject key_bookcard;
  public Transform[] children;
    void Start()
    {
        children = transform.GetComponentsInChildren<Transform>(true);
         //Debug.Log(children[1].gameObject.name);
    }

    void Update()
    {
      if (Input.GetMouseButtonDown(0)){
        OnClick();
        children[1].gameObject.SetActive(false);
      }
      
    }

    void OnClick()
    {
        Once = true;
        //Transform[] children = transform.GetComponentsInChildren<Transform>();
        if(Once==true && children[1].gameObject.activeSelf==true ){
            children[2].gameObject.SetActive(true);
            key_bookcard=children[3].gameObject;
            StartCoroutine(bcmoving());
            //Once=false;
        }
        
    
    }
    public IEnumerator bcmoving()
    {
        Vector3 startposition = key_bookcard.transform.position;
        Vector3 endposition = new Vector3(startposition.x, startposition.y + 90, startposition.z);
        float elapsedTime = 0f;
        float time = 0.5f;
        while (elapsedTime < time)
        {
            key_bookcard.transform.position = Vector3.Lerp(startposition, endposition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        key_bookcard.transform.position = endposition;
        Debug.Log("Done");
    }//아이템 넣고 퍼즐 누르면 끝나야 함
}

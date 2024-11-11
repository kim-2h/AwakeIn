using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBookCover : MonoBehaviour
{
    BookCoverManager bookCoverManager;
    bool Once = false;
    public GameObject bookinside;
    public GameObject InvenManager;
    public GameObject key_bookcard;
  public Transform[] children;
    void Start()
    {
        children = transform.GetComponentsInChildren<Transform>(true);
         //Debug.Log(children[1].gameObject.name);
    }

   
   public void ThroughPages()
    {
        SoundManager.Instance.PlaySFX(8);
        Once = true;
        //Transform[] children = transform.GetComponentsInChildren<Transform>();
        if(Once==true && gameObject.activeSelf==true ){
            bookinside.SetActive(true);
            StartCoroutine(bcmoving());
            Once=false;
        }
        
    
    }
    public void KeyCLicked(){
        InvenManager.GetComponent<InvenManager>().ItemAdder("BookCard");
        //bookCoverManager.gameObject.GetComponent<IPuzzle>().IsSolved = true;
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

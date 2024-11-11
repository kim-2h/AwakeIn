using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChalkboardManager : MonoBehaviour
{
    RaycastHit hit;
    
    private Vector3 CameraPosition;

    public DialogueManager dialogueManager;
    public bool IsDropped=false;
    public Material material;
    public Sprite sprite;
    public GameObject other1, other2, other3;
    public InvenManager invenManager;
    
    void Start()
    {
        CameraPosition = Camera.main.gameObject.transform.position;
    }
    
    public void OnMouseDown()
    {
        if (Camera.main.gameObject.transform.position != CameraPosition || Time.timeScale == 0)
        {
            return;
        }

        if (other1.activeInHierarchy || other2.activeInHierarchy || other3.activeInHierarchy)
        {
            return;
        }

        if (!IsDropped && (invenManager.ItemMap["Bird_Note"].InInventory || invenManager.ItemMap["Bird_Note"].IsUsed))
        {
            dialogueManager.Panel.SetActive(true);
          StartCoroutine(dialogueManager.PlayDialogue("Huh, I can reach up to here."));
           
          StartCoroutine(DropingBoard());
        }
            
       
    }

    IEnumerator DropingBoard(){
       
        yield return new WaitForSeconds(2);
      Vector3 initialposition=transform.position; 
        Vector3 targetposition=new Vector3(transform.position.x, transform.position.y-2.2f, transform.position.z);
        float elapsedTime=0f;
        float duration=2f;
         while (elapsedTime < duration)
        {   SoundManager.Instance.PlaySFX(4);
            //float progress = elapsedTime / duration;
          transform.position=Vector3.Lerp(initialposition,targetposition, elapsedTime/duration);
           elapsedTime+=Time.deltaTime;
            //Debug.Log(Train.transform.position);
    }
    gameObject.GetComponent<SpriteRenderer>().sprite=sprite;
    IsDropped=true;
}
}
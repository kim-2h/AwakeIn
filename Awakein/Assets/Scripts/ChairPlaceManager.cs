using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairPlaceManager : MonoBehaviour
{   
    public InvenManager InvenManager;
    public GameObject Chair;
    public GameObject[] Places;
    public enum ChairState
    {Nowhere, BookShelf, PhotoFrame, Clock};
    public ChairState ChairNow = ChairState.Nowhere;

    public void ChairDragBegin()
    {
        Places[0].gameObject.SetActive(true); 
        Places[1].gameObject.SetActive(true); 
        Places[2].gameObject.SetActive(true);
    }
    public void ChairOnDrag()
    {

    }
    public void ChairDragEnded()
    {
        Places[0].gameObject.SetActive(false); 
        Places[1].gameObject.SetActive(false); 
        Places[2].gameObject.SetActive(false);
    }
    public void ChairDragEnd(RaycastHit Hit)
    {
        Places[0].gameObject.SetActive(false); 
        Places[1].gameObject.SetActive(false); 
        Places[2].gameObject.SetActive(false);

        Debug.Log("chair placed on " + Hit.transform.name);
        if (Hit.transform.parent.name == "ChairNPlaceholder")
        {
            InvenManager.RemoveItem("Chair");
            Chair.gameObject.SetActive(true);
            Chair.transform.position = Hit.transform.GetChild(0).position;
            int t = Hit.transform.name[10] - 48;
            Debug.Log(t);
            switch (t)
            {
                case 1:
                    ChairNow = ChairState.BookShelf;
                    SoundManager.Instance.PlaySFX(5);
                    Debug.Log("chair now BookShelf");
                    break;
                case 2:
                    ChairNow = ChairState.PhotoFrame;
                    SoundManager.Instance.PlaySFX(5);
                    Debug.Log("chair now PhotoFrame");
                    break;
                case 3:
                    ChairNow = ChairState.Clock;
                    SoundManager.Instance.PlaySFX(5);
                    Debug.Log("chair now Clock");
                    break;
            }
        }
        else
        {
            ChairNow = ChairState.Nowhere;
        }
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

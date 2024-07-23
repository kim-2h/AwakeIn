using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class chair_arranging : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Vector3 originalpos;
    [SerializeField]
    private GameObject[] place2;
    private Vector3 currentpos1;
    public Vector3 currentpos2;
 
    GameObject realchair;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        place2 = GameObject.FindGameObjectsWithTag("chairplace");
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        originalpos = gameObject.transform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        
        //currentpos1 = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
        //currentpos1.z = 0; // Assuming 2D
        gameObject.transform.position = currentpos2=eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
       
        // Use the world position of currentpos
        Vector3 position = Camera.main.WorldToScreenPoint(place2[0].transform.position);
Vector3 position1 = Camera.main.WorldToScreenPoint(place2[1].transform.position);
Vector3 position2= Camera.main.WorldToScreenPoint(place2[2].transform.position);
         Debug.Log(position.x);//3d상의 위치
        if ( position.x - 10 <= currentpos2.x && currentpos2.x <= position.x + 10)
        {
            realchair = Instantiate(prefab, place2[0].transform.position, Quaternion.identity);
            gameObject.transform.position=originalpos;
        }
        else if ( position1.x - 10 <= currentpos2.x && currentpos2.x <= position1.x + 10)
        {
            realchair = Instantiate(prefab, place2[1].transform.position, Quaternion.identity);
            gameObject.transform.position=originalpos;
        }
        else if ( position2.x - 10 <= currentpos2.x && currentpos2.x <= position2.x + 10)
        {
            realchair = Instantiate(prefab, place2[2].transform.position, Quaternion.identity);
            gameObject.transform.position=originalpos;
        }
        else
        {
            // Revert to original position if not within range
            gameObject.transform.position = originalpos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

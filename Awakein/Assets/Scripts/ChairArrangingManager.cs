using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChairArrangingManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 originalpos;
    
    public RawImage[] InvenSlots;
    GameObject realchair;
    public Texture chairTexture; 
    public GameObject prefab;
    bool Isfind=false;
    void Start()
    {
        InvenSlots = transform.GetComponentsInChildren<RawImage>();
       
    }
    void Update(){
        foreach (RawImage rawImage in InvenSlots)
        {
            if ( rawImage.texture == chairTexture&&Isfind==false)
            {
                Debug.Log(rawImage.texture);
                ChairDragHandler handler = rawImage.gameObject.AddComponent<ChairDragHandler>();
                handler.prefab = prefab; 
                Debug.Log("Added");
                Isfind=true;
            }
        }
    }
}

public class ChairDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private GameObject[] place2;
    GameObject realchair;
    public GameObject prefab;

    private Vector3 originalpos;
    private Vector3 currentpos2;

    void Start()
    {
        place2 = GameObject.FindGameObjectsWithTag("chairplace");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalpos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = currentpos2 = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(place2[0].transform.position);
        Vector3 position1 = Camera.main.ScreenToWorldPoint(place2[1].transform.position);
        Vector3 position2 = Camera.main.ScreenToWorldPoint(place2[2].transform.position);

        Debug.Log(position); 
         Debug.Log(position1); /// 3D상의 위치
          Debug.Log(position2); 
         Debug.Log(currentpos2); 
        if (position.x - 10 <= currentpos2.x && currentpos2.x <= position.x + 10)
        {
            realchair = Instantiate(prefab, place2[0].transform.position, Quaternion.identity);
            transform.position = originalpos;
        }
        else if (position1.x - 10<= currentpos2.x && currentpos2.x <= position1.x + 10)
        {
            realchair = Instantiate(prefab, place2[1].transform.position, Quaternion.identity);
            transform.position = originalpos;
        }
        else if (position2.x - 10<= currentpos2.x && currentpos2.x <= position2.x + 10)
        {
            realchair = Instantiate(prefab, place2[2].transform.position, Quaternion.identity);
            transform.position = originalpos;
        }
        else
        {
         
            transform.position = originalpos;
        }
    }
}

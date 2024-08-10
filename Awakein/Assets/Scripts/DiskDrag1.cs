using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiskDrag1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject Manager, RealManager;
    private Vector3 initialPosition;
    public GameObject RodTemp;
    private GameObject StartRod;
    private bool isValid = true;
    private float fadeSpeed=1.5f;
    private Vector3[] RoadPosition = {new Vector3(-400, 0, 0), new Vector3(0, 0, 0), new Vector3(800, 0, 0)};

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
        StartRod = transform.parent.gameObject;

        int childCount = transform.parent.childCount - 1;
        if (transform.parent.GetChild(childCount).gameObject != this.gameObject) //맨 위가 아니면 못움직임 
        {
            isValid = false;
        }
        else
        {
            isValid = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isValid)
        {
            transform.position = eventData.position;
            this.transform.SetParent(RodTemp.transform);
            //transform.position = new Vector3(eventData.position.x, eventData.position.y, Distttt);
        }
        else
        {
            transform.position = initialPosition;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isValid) //맨 위가 아니면 못움직임
        {
            Debug.Log("Invalid!can't move!");
            this.transform.position = initialPosition;
            this.transform.SetParent(StartRod.transform);
            return;
        }
        
        int finalPosition = SetFinalPosition(eventData.position);
        //범위 벗어나지 않았고 위치에 놓을수 있는지 확인
        if (finalPosition != 0 && Manager.gameObject.GetComponent<HanoiManagercopy>().DiskRuller(this.gameObject, finalPosition))
        {
            Manager.gameObject.GetComponent<HanoiManagercopy>().DiskPlacer(this.gameObject, finalPosition);
            Debug.Log("Valid!move to " + finalPosition);
        }
        else
        {
            Debug.Log("Invalid!can't too far!");
            transform.position = initialPosition;
            this.transform.SetParent(StartRod.transform);
        }

        if (Manager.gameObject.GetComponent<HanoiManagercopy>().Rods[2].transform.childCount == 4)
        {
            Debug.Log("~~~Game End!!!~~~");
            for (int i=0;i<4;i++){
                 StartCoroutine(FadeIn(Manager.gameObject.GetComponent<HanoiManagercopy>().texts[i]));
            }
            RealManager.gameObject.GetComponent<IPuzzle>().IsSolved = true;
            RealManager.gameObject.GetComponent<HanoiRealManager1>().StartPuzzle();
        }
    }
    private int SetFinalPosition(Vector3 position)
    {
        Vector3 diskPos = position;
        Debug.Log(diskPos);

        if (diskPos.x<-3000 || diskPos.x>3000|| diskPos.y<-1500 || diskPos.y>1500)
        {
            return 0;
        } 
        if (Vector3.SqrMagnitude(diskPos - RoadPosition[1]) > Vector3.SqrMagnitude(diskPos - RoadPosition[0]) &&
            Vector3.SqrMagnitude(diskPos - RoadPosition[2]) > Vector3.SqrMagnitude(diskPos - RoadPosition[0]))
        {
            return 1;
        }
        else if (Vector3.SqrMagnitude(diskPos - RoadPosition[0]) > Vector3.SqrMagnitude(diskPos - RoadPosition[1]) &&
                 Vector3.SqrMagnitude(diskPos -RoadPosition[2]) > Vector3.SqrMagnitude(diskPos - RoadPosition[1]))
        {
            return 2;
        }
        else if (Vector3.SqrMagnitude(diskPos - RoadPosition[0]) > Vector3.SqrMagnitude(diskPos - RoadPosition[2]) &&
                 Vector3.SqrMagnitude(diskPos -RoadPosition[1]) > Vector3.SqrMagnitude(diskPos - RoadPosition[2]))
        { 
            return 3;
        }
        else
        {
            Debug.Log("Invalid!can't too far! 222");
            return 0;
        }
    }

    void Start()
    {
       
        initialPosition = transform.position;
        RoadPosition[0] = Manager.GetComponent<HanoiManagercopy>().Rods[0].transform.position;
        RoadPosition[1] = Manager.GetComponent<HanoiManagercopy>().Rods[1].transform.position;
        RoadPosition[2] = Manager.GetComponent<HanoiManagercopy>().Rods[2].transform.position;
    }

    // Update is called once per frame
    
    IEnumerator FadeIn(Text tf){
        Debug.Log("StartCour");
      Color newColor = tf.color;           
        newColor.a = 0f;
       while(newColor.a<1f){
          newColor.a+=Time.deltaTime*fadeSpeed;
         tf.color = newColor;
         yield return null;
       }
     tf.gameObject.SetActive(true);
    }
}

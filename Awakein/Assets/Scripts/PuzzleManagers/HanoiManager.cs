using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class HanoiManager : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] public bool IsSolved { get; set; }
    [SerializeField] public List<GameObject> Disks = new List<GameObject>();
    public List<GameObject> Rods = new List<GameObject>();
    public Image[] texts;
    private Vector3[] RoadPosition = {new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)};
    private float[][] PlaceMap = new float[4][];


    public void InitDisk() //get rocation of first rod and place each disk on it
    {
        if (PlaceMap[0] == null) PlaceMap[0] = new float[] {-6, 0, 0, 0};
        if (PlaceMap[1] == null) PlaceMap[1] = new float[] {-7, 10, 0, 0};
        if (PlaceMap[2] == null) PlaceMap[2] = new float[] {-10, 4, 21, 0};
        if (PlaceMap[3] == null) PlaceMap[3] = new float[] {-12, -1, 13, 30};
        Vector3 rodPos = Rods[0].GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("init hanoi");
        for (int i = 0; i < 4; i++)
        {
            Disks[i].transform.SetParent(canvas.transform);
        }
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log(rodPos.y + i * 100 + "\n");
            //Disks[i].transform.position = new Vector3(rodPos.x, rodPos.y + i*85 + 180 - 10*i*i, rodPos.z);
            // Disks[i].transform.position = new Vector3(rodPos.x, rodPos.y + i*80 + 
            // 180 - 10*i*i, rodPos.z);
            Disks[i].transform.SetParent(Rods[0].transform);
            Disks[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, PlaceMap[i][i], 0);
            Debug.Log(Disks[i].transform.position.y);
            
        }
    }

    public bool DiskRuller(GameObject disk, int i) //check if disk can be placed on rod
    {
        GameObject rod = Rods[i-1];
        int childCount = rod.transform.childCount -1;
        if (rod.transform.childCount == 0)
        {
            return true;
        }
        else
        {
            if (disk.gameObject.name[4] > rod.transform.GetChild(childCount).gameObject.name[4])
            {
                Debug.Log("ruller true!");
                return true;
            }
            else
            {
                Debug.Log("ruller false!");
                return false;
            }
        }
    }

    public void DiskPlacer(GameObject disk, int i) //place disk on rod
    {
        GameObject rod = Rods[i-1];
        int Idx = rod.transform.childCount;
        int Width = (int)disk.GetComponent<RectTransform>().sizeDelta.x;
        var RodX = rod.GetComponent<RectTransform>().anchoredPosition.x;
        RodX = 0;
        // disk.transform.position = new Vector3(rod.transform.position.x, 
        // rod.transform.position.y + Idx * 85 + 180 + 50*(1-300/Width), rod.transform.position.z);
        disk.transform.SetParent(rod.transform);
        if (Width >=280)
        {
            disk.GetComponent<RectTransform>().anchoredPosition = 
            new Vector3(RodX, PlaceMap[0][Idx], 0);
        }
        else if (Width >= 180)
        {
            disk.GetComponent<RectTransform>().anchoredPosition = 
            new Vector3(RodX, PlaceMap[1][Idx], 0);
        }
        else if (Width >= 120)
        {
            disk.GetComponent<RectTransform>().anchoredPosition = 
            new Vector3(RodX, PlaceMap[2][Idx], 0);
        }
        else if (Width >= 80)
        {
            disk.GetComponent<RectTransform>().anchoredPosition =
             new Vector3(RodX, PlaceMap[3][Idx], 0);
        }
        
        
    }


    void Start()
    {
        
        // for (int i=0;i<4;i++){
        //         Color newColor = texts[i].color;  
        //        newColor.a = 0f; 
        //         texts[i].color = newColor; 
        //     }
        IsSolved = false;
        PlaceMap[0] = new float[] {-6, 0, 0, 0};
        PlaceMap[1] = new float[] {-7, 10, 0, 0};
        PlaceMap[2] = new float[] {-10, 4, 21, 0};
        PlaceMap[3] = new float[] {-12, -1, 13, 30};
        InitDisk();
    }

    public void StartAnime()
    {
        StartCoroutine(AnswerAnim());
    }
    public bool coroutineING = false;
    IEnumerator AnswerAnim()
    {
        coroutineING = true;
        float time = 2f;
        float elapsedTime = 0f;
        Color Transparent = new Color(255, 255, 255, 0);
        for (int i=0; i<4; i++)
        {
            Disks[i].transform.GetChild(0).gameObject.SetActive(false);
            Disks[i].transform.GetChild(1).gameObject.SetActive(true);
            Disks[i].transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Transparent;
        }

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            for (int i=0; i<4; i++)
            {
                Disks[i].transform.GetChild(1).gameObject.GetComponent<RawImage>().color = 
                Color.Lerp(Transparent, Color.white, elapsedTime/time);
            }


            yield return null;
        }
        coroutineING = false;

    }

    // Update is called once per frame
    /*
        디스크 위치 하드코딩으로 조정해야됨 ㅡㅡ
        디스크1은 항상 1층: -6
        디스크2: 1층: -7, 2층: 10, 3층
        디스크3: 1층: -10 2층: 4 3층: 21
        디스크4: 1층: -12, 2층: -1 3층: 13 4층: 30
    
    */
    
}

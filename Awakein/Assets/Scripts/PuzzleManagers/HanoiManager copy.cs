using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HanoiManagercopy : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] public bool IsSolved { get; set; }
    [SerializeField] public List<GameObject> Disks = new List<GameObject>();
    public List<GameObject> Rods = new List<GameObject>();
    public Text[] texts;
    private Vector3[] RoadPosition = {new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)};
    public void InitDisk() {
        Vector3 rodPos = Rods[0].transform.position;
        Debug.Log("init hanoi");
        for (int i = 0; i < 4; i++){
            Debug.Log(rodPos.y + i * 100 + "\n");
            Disks[i].transform.position = new Vector3(rodPos.x, rodPos.y + i*85 + 80, rodPos.z);
            Disks[i].transform.SetParent(Rods[0].transform);
        }
    }
    
    public bool DiskRuller(GameObject disk, int i) //check if disk can be placed on rod
    {
        GameObject rod = Rods[i-1];
        int childCount = rod.transform.childCount - 1;
        if (rod.transform.childCount == 0) {
            return true;
        }
        else
        {
            if (disk.gameObject.name[4] > rod.transform.GetChild(childCount).gameObject.name[4]) {
                Debug.Log("ruller true!");
                return true;
            }
            else{
                Debug.Log("ruller false!");
                return false;
            }
        }
    }
    public void DiskPlacer(GameObject disk, int i) {
        GameObject rod = Rods[i-1];
        disk.transform.position = new Vector3(rod.transform.position.x, 
        rod.transform.position.y + rod.transform.childCount * 85 + 80, 
        rod.transform.position.z);
        disk.transform.SetParent(rod.transform);
    }


    void Start()
    {
        InitDisk();
         for (int i=0;i<4;i++){
                Color newColor = texts[i].color;  
               newColor.a = 0f; 
                texts[i].color = newColor; 
            }
        IsSolved = false;
    }
}

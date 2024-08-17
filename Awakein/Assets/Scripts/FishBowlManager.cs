using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBowlManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    private Vector3 CameraPosition;
    public GameObject FishFood, InvenManager;
    public void StartPuzzle()
    {
        Debug.Log("FishBowl Puzzle Started");
        canvas.gameObject.SetActive(true);
        FishFood.GetComponent<Fish>().InitPuzzle();
    }
    public void ExitPuzzle()
    {
        Debug.Log("FishBowl Puzzle Exit");
        canvas.gameObject.SetActive(false);
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    public void KeyClicked()
    {
        IsSolved = true;
        InvenManager.GetComponent<InvenManager>().ItemAdder("DriverHandle");
    }

    void Start()
    {
        IsSolved = false;
        CameraPosition = Camera.main.gameObject.transform.position; 
        canvas.gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}

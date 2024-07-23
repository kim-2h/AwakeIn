using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelfManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    private Vector3 CameraPosition;

    public void StartPuzzle()
    {
        Debug.Log("BookShelf Puzzle Started");
        canvas.gameObject.SetActive(true);
    }
    public void ExitPuzzle()
    {
        Debug.Log("BookShelf Puzzle Exit");
        canvas.gameObject.SetActive(false);
        Camera.main.gameObject.transform.position = CameraPosition;
    }
    void Start()
    {
        CameraPosition = Camera.main.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HanoiRealManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    public HanoiManager Manager;
    private Vector3 CameraPosition;
    public void StartPuzzle()
    {
        Debug.Log("Hanoi Puzzle Started");
        canvas.gameObject.SetActive(true);
        if (IsSolved)
        {
            foreach (GameObject disk in Manager.Disks)
            {
                disk.gameObject.GetComponent<RawImage>().raycastTarget = false;
            }
        }
        else
        {
            Manager.InitDisk();
        }
    }
    public void ExitPuzzle()
    {
        if (canvas.gameObject.activeInHierarchy)
        {
            Debug.Log("Hanoi Puzzle Exit");
            canvas.gameObject.SetActive(false);
            Camera.main.gameObject.transform.position = CameraPosition;
        }

    }
        void Start()
    {
        IsSolved = false;
        CameraPosition = Camera.main.gameObject.transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadioManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    public GameObject Radio;
    private Vector3 CameraPosition;
    public void StartPuzzle()
    {
        Debug.Log("Radio Puzzle Started");
        canvas.gameObject.SetActive(true);
    }
    public void ExitPuzzle()
    {
        Debug.Log("Radio Puzzle Exit");
        canvas.gameObject.SetActive(false);
        Radio.GetComponent<DialControl>().RadioReset();
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    void Start()
    {
        IsSolved = false;
        CameraPosition = Camera.main.gameObject.transform.position;
    }
}

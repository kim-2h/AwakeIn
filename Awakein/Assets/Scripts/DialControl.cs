using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;


public class DialControl: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 Origin = new Vector3(250, -100, 0);
    private Vector3 InitialRotation; 
    private Vector3 InitialPosition;
    private Vector2 CurrentMouseP;
    private Vector3 CurrentRotation;
    

    public List<int> Numbers = new List<int>(25);
    public int Offset = 15;
    public int NumBefore = -1;
    public Button button;
    public Slider slider;
    public int[] Password = { 12, 9, 12, 3, 12, 9, 6, 0};
    public GameObject RadioManager;
   
    private bool isSolved = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        InitialRotation = transform.eulerAngles;
        InitialPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    }
    public void OnDrag(PointerEventData eventData)
    {
        CurrentMouseP = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        CurrentRotation = new Vector3(0, 0, (Mathf.Atan2(CurrentMouseP.y - Origin.y, CurrentMouseP.x - Origin.x) -
            Mathf.Atan2(InitialPosition.y - Origin.y, InitialPosition.x - Origin.x)) * Mathf.Rad2Deg);

        float newZRotation = (CurrentRotation + InitialRotation).z;
        if (newZRotation < 0)
        {
            newZRotation += 360;
        }
        newZRotation = newZRotation % 360;

        transform.eulerAngles = new Vector3(0, 0, newZRotation);

        if ((newZRotation >= 0 && newZRotation < Offset) || (newZRotation <= 360 && newZRotation > 360 - Offset))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            if (NumBefore != 12 && Numbers.Count < Password.Length)
            {
                Numbers.Add(12);
                Debug.Log("12");
                NumBefore = 12;
                if (Numbers[Numbers.Count - 1] == Password[Numbers.Count - 1])
                {
                    SliderChange();
                }
            }
        }
        else if (newZRotation >= 90 - Offset && newZRotation <= 90 + Offset)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            if (NumBefore != 9 && Numbers.Count < Password.Length)
            {
                Numbers.Add(9);
                Debug.Log("9");
                NumBefore = 9;
                if (Numbers[Numbers.Count - 1] == Password[Numbers.Count - 1])
                {
                    SliderChange();
                }
            }
        }
        else if (newZRotation >= 180 - Offset && newZRotation <= 180 + Offset)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            if (NumBefore != 6 && Numbers.Count < Password.Length)
            {
                Numbers.Add(6);
                Debug.Log("6");
                NumBefore = 6;
                if (Numbers[Numbers.Count - 1] == Password[Numbers.Count - 1])
                {
                    SliderChange();
                }
            }
        }
        else if (newZRotation >= 270 - Offset && newZRotation <= 270 + Offset)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            if (NumBefore != 3 && Numbers.Count < Password.Length)
            {
                Numbers.Add(3);
                Debug.Log("3");
                NumBefore = 3;
                if (Numbers[Numbers.Count - 1] == Password[Numbers.Count - 1])
                {
                    SliderChange();
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //transform.eulerAngles = CurrentRotation + InitialRotation;
    }

    public void ButtonClick()
    {
        int correct = 0;
        Debug.Log("Button Clicked");
        if (Numbers.Count >= Password.Length)
        {
            Debug.Log("invalid Password");
            correct = 0;
        }
        else
        {
            for (int i = 0; i < Password.Length-1 && Numbers.Count >i; i++)
            {
                if (Password[i] != Numbers[i])
                {
                    Debug.Log("Wrong Password");
                    correct = 0;
                    break;
                }
                else if (i == Password.Length - 2)
                {
                    correct = 1;
                }
            }
            if (correct == 1)
            {
                Debug.Log("Correct Password");
                this.gameObject.GetComponent<RawImage>().raycastTarget = false;
                RadioManager.gameObject.GetComponent<RadioManager>().DriverHandle.SetActive(!RadioManager.gameObject.GetComponent<IPuzzle>().IsSolved);
                RadioManager.gameObject.GetComponent<IPuzzle>().IsSolved = true;
                isSolved = true;
                
            }
            else
            {
                Debug.Log("Wrong Password");
            }
        }
    }
    public void SliderChange()
    {
        //slider.value += 360/Password.Length;
        StartCoroutine(SliderUp(360/(Password.Length-1)));
    }
    public void RadioReset()
    {
        if (!isSolved)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            Numbers.Clear();
            NumBefore = -1;
            slider.value = -10;
        }
        else
        {
            this.gameObject.GetComponent<RawImage>().raycastTarget = false;
            RadioManager.gameObject.GetComponent<IPuzzle>().IsSolved = true;
        }
    }
    IEnumerator SliderUp( double K)
    {
        for (int i = 0; i <= K+5; i++)
        {
            slider.value += 1;
            yield return null;
        }
        for (int i = 0; i <= 10; i++)
        {
            slider.value -= 1;
            yield return null;
        }
        for (int i = 0; i <= 5; i++)
        {
            slider.value += 1;
            yield return null;
        }
        
    }

    void Start()
    {
        Origin = this.transform.position;
        Numbers.Clear();
        NumBefore = -1;
    }
    void Update()
    {
        
    }
}

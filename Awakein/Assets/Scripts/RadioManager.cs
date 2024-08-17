using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class RadioManager : MonoBehaviour, IPuzzle
{
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    public GameObject InvenManager, Radio, RadioFront, RadioBack, Battery, BatterySlot, DriverHandle;
    private Vector3 CameraPosition, BatteryPosition;
    private bool BatteryIn = false;

    public void StartPuzzle()
    {
        Debug.Log("Radio Puzzle Started");
        canvas.gameObject.SetActive(true);
        Radio.gameObject.GetComponent<RawImage>().raycastTarget = BatteryIn;
        RadioFront.SetActive(true);
        RadioBack.SetActive(false);
        DriverHandle.SetActive(IsSolved && !InvenManager.GetComponent<InvenManager>().ItemMap["DriverStick"].InInventory && !InvenManager.GetComponent<InvenManager>().ItemMap["DriverStick"].IsUsed);
    }
    public void ExitPuzzle()
    {
        Debug.Log("Radio Puzzle Exit");
        canvas.gameObject.SetActive(false);
        Radio.GetComponent<DialControl>().RadioReset();
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    public void RadioClicked()
    {
        if (InvenManager.GetComponent<InvenManager>().ItemMap["RadioBattery"].InInventory)
        {
            Battery.gameObject.SetActive(true);
        }
        else
        {
            Battery.gameObject.SetActive(false);
        }
        if (RadioFront.activeSelf)
        {
            RadioFront.SetActive(false);
            RadioBack.SetActive(true);
        }
        else if (RadioBack.activeSelf)
        {
            RadioFront.SetActive(true);
            RadioBack.SetActive(false);

        }
        else
        {
            RadioFront.SetActive(false);
            RadioBack.SetActive(true);
        }
    }

    void Start()
    {
        IsSolved = false;
        Radio.gameObject.GetComponent<RawImage>().raycastTarget = false;
        //BatteryPosition = Battery.transform.position;
        canvas.gameObject.SetActive(false);
        CameraPosition = Camera.main.gameObject.transform.position;
        //var Rect = canvas.GetComponent<RectTransform>();
        BatteryPosition = BatteryRect.anchoredPosition;
    }

    //라디오 뒷면 함수도 여기다 넣음

    private RectTransform BatteryRect => Battery.GetComponent<RectTransform>();
    public void BeginDrag()
    {
        Battery.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void Drag()
    {
        BatteryRect.position = Input.mousePosition;
    }
    public void Drop() //슬롯에 들어갔나 확인
    {
        Debug.Log("Battery Drop");
        //Rect BatterySlotRect = BatterySlot.gameObject.GetComponent<RectTransform>().rect;
        //Rect BatteryRect = Battery.gameObject.GetComponent<RectTransform>().rect;
        //Vector3 mousePos = Input.mousePosition;

        if (Vector3.Distance(BatterySlot.transform.position, Input.mousePosition) < 80)
        {
            BatteryIn = true;
            Battery.gameObject.transform.position = BatterySlot.transform.position;
            Radio.gameObject.GetComponent<RawImage>().raycastTarget = true;
            InvenManager.GetComponent<InvenManager>().ItemMap["RadioBattery"].IsUsed = true;
            InvenManager.GetComponent<InvenManager>().RemoveItem("RadioBattery");
            Battery.gameObject.GetComponent<RawImage>().raycastTarget = false;
        }
        else
        {
            Debug.Log("Battery Drop Failed");
            Battery.transform.position = new Vector3(0, 0, 0);
            BatteryRect.anchoredPosition = BatteryPosition;

            Battery.transform.rotation = Quaternion.Euler(0, 0, -90);
        }

    }

    public void DriverStickClicked()
    {
        InvenManager.GetComponent<InvenManager>().ItemAdder("DriverStick");
        DriverHandle.SetActive(false);
    }

}

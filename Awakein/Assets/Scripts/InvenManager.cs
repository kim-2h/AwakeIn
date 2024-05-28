using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;

public class InvenManager : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /*
    인벤토리에 아이템이 들어가는 것을 관리하는 스크립트
    아이템 클릭, 스택으로 인벤토리에 추가, 아이템 사용, 인벤토리 이미지 등 관리
    아이템의 속성 자체는 ItemManager에서 관리
    */
    public List<Item> ItemList = new List<Item>();
    public GameObject Popup;

    [SerializeField] public Button[] Slots;
    [SerializeField] public int[] SlotOccu = {0,0,0,0,0,0,0,0,0,0,0,0}; 
    public int SlotNum = 12;
    public GameObject selectedObject;
    private GameObject TempSlot;
    public int NowMove = -1;
    public void SlotClicked()
    {
        string sName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(sName);
        NowMove = (sName[9] - '0')*10 + sName[10]-'0'; // Slot[0] ~ Slot[11
        selectedObject = Slots[NowMove].gameObject;
        OpenPopup(selectedObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text);
    }
    public void ItemAdder(string iName)
    {
        Debug.Log(iName + " added");
        
        for (int i = 0; i<Slots.Length && SlotNum >0; i++)
        {
            TextMeshProUGUI Text = Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(); 
            if (SlotOccu[i] == 0 &&(Text.text == null || Text.text == ""))
            {
                Text.text = iName;
                SlotNum--;
                SlotOccu[i] = 1;
                GameObject[] var = GameObject.FindGameObjectsWithTag("Item");
                foreach (GameObject item in var)
                {
                    if (item.name == iName)
                    {  //Assets/CustomSprites/Key1.png 
                        string path = "Assets/CustomSprites/" + iName + ".png";
                        Texture newSprite = AssetDatabase.LoadAssetAtPath<Texture>(path);
                        if (newSprite == null)
                        {
                            Debug.Log("Sprite not found at path: " + path);
                        }
                        else
                        {
                            Slots[i].GetComponent<RawImage>().texture = newSprite;
                            Slots[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                            item.SetActive(false);
                        }
  
                    }
                }
                break;
            }
        }
    }

    Vector3 initialPosition;

    public void OpenPopup(string iName)
    {
        Item Item = null;
        foreach (Item item in ItemList)
        {
            if (item.ItemName == iName)
            {
                Debug.Log("Item Found");
                Item = item;
                break;
            }
        }
        if (!Item)
        {
            Debug.Log("Item Not Found");
        }
        else
        {
            if (Item.IsClickable)
            {
                Popup.SetActive(true);
            }
        }
        
    
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent<Button>(out Button component) && 
        Slots.Contains(component))
        {
            SlotClicked();
            selectedObject = Slots[NowMove].gameObject;
            initialPosition = selectedObject.transform.position;
            temp = Instantiate(selectedObject);
            temp.transform.SetParent(TempSlot.gameObject.transform);
            temp.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            TempSlot.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
            TempSlot.GetComponent<RawImage>().texture = selectedObject.GetComponent<RawImage>().texture;
            Debug.Log("Begin Drag : " + selectedObject.name);
        }
        else
        {
            Debug.Log("Not Slot");
        }

    }
    public GameObject temp;
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        if (SlotOccu[NowMove] != 1)
        {
            return;
        }
        //TempSlot.transform.position = Input.mousePosition;
        selectedObject.transform.position = Input.mousePosition;
        temp.transform.SetParent(TempSlot.transform);
        TempSlot.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        selectedObject.transform.position = initialPosition;
        TempSlot.transform.position = new Vector3(-10000, -10000, 0);
        TempSlot.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);

        DestroyImmediate(temp);
        Debug.Log("End Drag");
    }
    public void SlotValidity(string sName)
    {
        //슬롯이 비어있지 않다면 아이템 사용
    }
    void Start()
    {
        NowMove = -1;
        for (int i = 0; i<Slots.Length; i++)
        {
            Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            SlotOccu[i] = 0;
            //Slots[i].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        TempSlot = transform.parent.Find("TempSlot").gameObject;
        TempSlot.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        Popup.SetActive(false);
    }
    void Update()
    {
        
    }
}

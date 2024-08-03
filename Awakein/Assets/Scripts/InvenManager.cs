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
using Unity.Mathematics;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class InvenManager : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /*
    인벤토리에 아이템이 들어가는 것을 관리하는 스크립트
    아이템 클릭, 스택으로 인벤토리에 추가, 아이템 사용, 인벤토리 이미지 등 관리
    아이템의 속성 자체는 ItemManager에서 관리
    */
    //public List<Item> ItemList = new List<Item>();
    public Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();
    public GameObject GameFlow;
    public GameObject Popup;
    public GameObject ItemManager;
    private Vector2 initialClick;
    private Vector2 finalClick;
    [SerializeField] public Button[] Slots;
    [SerializeField] public int[] SlotOccu = {0,0,0,0,0,0,0,0,0,0,0,0}; 
    public int SlotNum = 12;
    public GameObject selectedObject;
    private GameObject TempSlot;
    public int NowMove = -1;
    private ChairPlaceManager ChairPlaceManager;
    public void SlotClicked()
    {
        string sName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(sName);
        NowMove = (sName[9] - '0')*10 + sName[10]-'0'; // Slot[0] ~ Slot[11
        selectedObject = Slots[NowMove].gameObject;
        var Text = selectedObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        if (Text != null && Text != "")
        {
            OpenPopup(Text);
        }
        
    }
    public void ItemAdder(string iName)
    {
        if (iName == "Radio" || iName == "Clock" || 
          iName == "PhotoFrame" || iName == "OrgelBody") 
        {
            if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.Nowhere)
            {
                return;
            }
            else if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.BookShelf)
            {
                if (iName != "Radio" && iName != "OrgelBody")
                {
                    return;
                }
            }
            else if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.PhotoFrame)
            {
                if (iName != "PhotoFrame")
                {
                    return;
                }
            }
            else if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.Clock)
            {
                if (iName != "Clock")
                {
                    return;
                }
            }
        }  
        Debug.Log(iName + " added");
        if (iName == "Chair")
        {
            ChairPlaceManager.ChairNow = ChairPlaceManager.ChairState.Nowhere;
        }
        for (int i = 0; i<Slots.Length && SlotNum >0; i++)
        {
            TextMeshProUGUI Text = Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(); 
            if (SlotOccu[i] == 0 &&(Text.text == null || Text.text == ""))
            {
                Text.text = iName;
                SlotNum--;
                SlotOccu[i] = 1;
                GameObject[] var = GameObject.FindGameObjectsWithTag("Item");
                //클릭된 아이템의 이름을 받아와서, 월드에 그런 아이템이 있는지 확인 후 인벤에 추가. 월드에서는 비활성화
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
                            ItemMap[iName].InInventory = true;
                            item.SetActive(false);
                        }
  
                    }
                }
                break;
            }
        }
    }

    Vector2 initialPosition;
    public void RemoveItem(string iName)
    {
        for (int i = 0; i<Slots.Length; i++)
        {
            if (Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text == iName)
            {
                Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
                SlotOccu[i] = 0;
                SlotNum++;
                Slots[i].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                break;
            }
        }
        ItemMap[iName].InInventory = false;
    }
    public void OpenPopup(string iName)
    {
        Item Item = null;
        // foreach (Item item in ItemList)
        // {
        //     if (item.ItemName == iName)
        //     {
        //         Debug.Log("Item Found");
        //         Item = item;
        //         break;
        //     }
        // }
        Item = ItemMap[iName];
        if (!Item)
        {
            Debug.Log("Item Not Found");
        }
        else
        {
            if (Item.IsClickable)
            {
                ItemManager.GetComponent<ItemManager>().OpenPopUp(iName);
                //Popup.SetActive(true); //일단 아이템 클릭하면 나오는 팝업은 하나인데. 이제 아이템매니저 
                //만들어서 아이템 효과 나타나게, 혹은 아이템=퍼즐이라면 퍼즐 인터페이스 달아서 그냥 3d세상에서 퍼즐 클릭한것처럼 얘를위한 팝업이 열리게 해야함
            }
        }
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemManager.GetComponent<ItemManager>().ClickNotDrag = false;
        Debug.Log("Begin Drag " + initialClick);
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent<Button>(out Button component) && Slots.Contains(component))
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

        if (selectedObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text == "Chair")
        {
            ChairPlaceManager.ChairDragBegin();
        }

    }
    public GameObject temp;
    public void OnDrag(PointerEventData eventData)
    {
        ItemManager.GetComponent<ItemManager>().ClickNotDrag = false;
        //Debug.Log("Dragging");
        //finalClick = eventData.position;
        //Popup.SetActive(false); //이부분ㅅㅂ 왜 distance로 하면 안먹히지 일케하면 느린데; 
        //ItemManager.GetComponent<ItemManager>().ClosePopUp(0);
        if (SlotOccu[NowMove] != 1)
        {
            return;
        }
        TempSlot.transform.position = Input.mousePosition;
        /*
        if (Vector2.Distance(initialClick, finalClick) > 10)
        {   
 
        } */ //distance로 구현하는부분 
        selectedObject.transform.position = Input.mousePosition;
        temp.transform.SetParent(TempSlot.transform);
        TempSlot.transform.position = Input.mousePosition;



    }
    public void OnEndDrag(PointerEventData eventData)
    {
        finalClick = eventData.position;
        Debug.Log("End Drag " + finalClick);
        //Debug.Log("Offset " + Vector2.Distance(initialClick, finalClick));
        selectedObject.transform.position = initialPosition;
        TempSlot.transform.position = new Vector3(-10000, -10000, 0);
        TempSlot.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);

        /* if (Vector2.Distance(initialClick, finalClick) < 1) //distance로 해서 클릭 드래그 구분하는거
         {
             OpenPopup(selectedObject.transform.name);
         } */

        // //아이템 위에 드랍했을 때
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition); 
        Debug.DrawRay(ray2.origin, ray2.direction * 100, Color.blue, 10f);
        Debug.Log("Raycast " + ray2);
        if (Physics.Raycast(ray2, out hit2, 100f))
        {
            if (hit2.transform.tag == "Puzzle")
            {
                Debug.Log("Item interacted: " + hit2.transform.name);           
            }
            else if (selectedObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text == "Chair")
            {
                Debug.Log("Chair Managing start");
                ChairPlaceManager.ChairDragEnd(hit2);
            }
        }
        else
        {
            if (selectedObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text == "Chair")
            {
                ChairPlaceManager.ChairDragEnded();
            }
        }
        ItemManager.GetComponent<ItemManager>().ClickNotDrag = true;
        DestroyImmediate(temp);
        Debug.Log("End Drag");
    }
    public void SlotValidity(string sName)
    {
        //슬롯이 비어있지 않다면 아이템 사용
    }
    public void ClosePopup()
    {
        Popup.SetActive(false);
    }
    void Start()
    {
        ItemMap = GameFlow.GetComponent<GameFlowManager>().ItemMap;
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
        ChairPlaceManager = GameObject.Find("Room1").transform.Find("ChairNPlaceholder").GetComponent<ChairPlaceManager>();
    }
    void Update()
    {
        
    }
}

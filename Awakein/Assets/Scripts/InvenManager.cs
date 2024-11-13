using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
//using UnityEditor.VersionControl;
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
using UnityEngine.SceneManagement;
using System.ComponentModel.Design.Serialization;
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
    private CombiningManager CombiningManager;



    void Awake()
    {

    }
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
                GameFlow.GetComponent<GameFlowManager>().CannotReach();
                return;
            }
            else if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.BookShelf)
            {
                if (iName != "Radio" && iName != "OrgelBody")
                {
                    GameFlow.GetComponent<GameFlowManager>().CannotReach();
                    return;
                }
            }
            else if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.PhotoFrame)
            {
                if (iName != "PhotoFrame")
                {
                    GameFlow.GetComponent<GameFlowManager>().CannotReach();
                    return;
                }
            }
            else if (ChairPlaceManager.ChairNow == ChairPlaceManager.ChairState.Clock)
            {
                if (iName != "Clock")
                {
                    GameFlow.GetComponent<GameFlowManager>().CannotReach();
                    return;
                }
            }
        }  
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
                //클릭된 아이템의 이름을 받아와서, 월드에 그런 아이템이 있는지 확인 후 인벤에 추가. 월드에서는 비활성화
                foreach (GameObject item in var)
                {
                    if (item.name == iName)
                    {  //여기 아래 두줄은 에디터상에서만 이용가능함
                        // string path = "Assets/CustomSprites/" + iName + ".png";
                        // Texture newSprite = AssetDatabase.LoadAssetAtPath<Texture>(path);
                        //ㄴㅇㅇㅇ 빌드할때는 다른 코드 써야함
                        // string path = "/CustomSprites/" + iName;
                        // Texture newSprite = Resources.Load(path) as Texture;
                        #if UNITY_EDITOR
                            string path = "Assets/CustomSprites/" + iName + ".png";
                            Sprite newSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                        #else
                            string path = "CustomSprites/" + iName;
                            Sprite newSprite = Resources.Load<Sprite>(path);
                        #endif
                        if (newSprite == null)
                        {
                            Debug.Log("Sprite not found at path: " + path);
                        }
                        else
                        {
                            // Slots[i].GetComponent<RawImage>().texture = newSprite;
                            // Slots[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                            Slots[i].GetComponent<Image>().sprite = newSprite;
                            Slots[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                            Slots[i].GetComponent<Image>().preserveAspect = true;
                            
                            ItemMap[iName].InInventory = true;
                            item.SetActive(false);
                        }
  
                    }
                }
                break;
            }
        }
        if (iName == "Chair")
        {
            GameFlow.GetComponent<GameFlowManager>().ChairBreaking();
            ChairPlaceManager.ChairNow = ChairPlaceManager.ChairState.Nowhere;
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
                Slots[i].GetComponent<Image>().sprite = null;
                Slots[i].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);
                break;
            }
        }
        ItemMap[iName].InInventory = false;
        ItemMap[iName].IsUsed = true;
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
            CombiningManager.OnButtonClicked();
            if (Item.IsClickable)
            {
                ItemManager.GetComponent<ItemManager>().OpenPopUp(iName);
                //Popup.SetActive(true); 
            }
        }
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemManager.GetComponent<ItemManager>().ClickNotDrag = false;
        CombiningManager.ClickNotDrag = false;
        //Debug.Log("Begin Drag " + initialClick);

        
        // if (NowMove == -1 || SlotOccu[NowMove] != 1)
        // {
        //     return;
        // }
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent<Button>(out Button component) && Slots.Contains(component))
        {
            SlotClicked();
            selectedObject = Slots[NowMove].gameObject;
            initialPosition = selectedObject.transform.position;
            temp = Instantiate(selectedObject);
            //TempSlot.transform.position = Input.mousePosition;
            temp.transform.SetParent(TempSlot.gameObject.transform);
            temp.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            TempSlot.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            TempSlot.GetComponent<Image>().sprite = selectedObject.GetComponent<Image>().sprite;
            TempSlot.GetComponent<Image>().preserveAspect = true;
            Debug.Log("Begin Drag : " + temp.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text);
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
        if (Input.mousePosition.y <5 || Input.mousePosition.y > 1075)
        {
            return;
        }
        ItemManager.GetComponent<ItemManager>().ClickNotDrag = false;
        CombiningManager.ClickNotDrag = false;
        
        //Debug.Log("Dragging");
        //finalClick = eventData.position;
        //Popup.SetActive(false); //이부분ㅅㅂ 왜 distance로 하면 안먹히지 일케하면 느린데; 
        //ItemManager.GetComponent<ItemManager>().ClosePopUp(0);
        if (NowMove == -1 || SlotOccu[NowMove] != 1)
        {
            return;
        }
        TempSlot.transform.position = Input.mousePosition;
        /*
        if (Vector2.Distance(initialClick, finalClick) > 10)
        {   
 
        } */ //distance로 구현하는부분 
        selectedObject.transform.position = Input.mousePosition;
        //temp.transform.SetParent(TempSlot.transform);
        //TempSlot.transform.position = Input.mousePosition;



    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (NowMove == -1 || SlotOccu[NowMove] != 1)
        {
            return;
        }
        finalClick = eventData.position;
        Debug.Log("End Drag " + finalClick);
        ItemManager.GetComponent<ItemManager>().ClickNotDrag = true;
        CombiningManager.ClickNotDrag = true;
        //Debug.Log("Offset " + Vector2.Distance(initialClick, finalClick));
        selectedObject.transform.position = initialPosition;
        TempSlot.transform.position = new Vector3(-10000, -10000, 0);
        TempSlot.GetComponent<Image>().color = new Color(0, 0, 0, 0);

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
                Debug.Log("Chair Managing dropped");
                ChairPlaceManager.ChairDragEnded();
            }
        }
        if (ChairPlaceManager != null) ChairPlaceManager.ChairDragEnded();
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
        TempSlot.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        Popup.SetActive(false);
        CombiningManager = transform.GetComponent<CombiningManager>();
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "2hBuildTest2" || scene == "SecondRoom") ChairPlaceManager = GameObject.Find("Room1").transform.Find("ChairNPlaceholder").GetComponent<ChairPlaceManager>();
        if (scene == "2hRoom2Backup") LoadInven();
    }

    public void SaveInven()
    {
        for (int i = 0; i<Slots.Length; i++)
        {
            PlayerPrefs.SetInt("SlotOccu" + i.ToString("D2"), SlotOccu[i]);
            PlayerPrefs.SetString("Slot" + i.ToString("D2"), Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text);
            Debug.Log("Slot " + i + "  with " + PlayerPrefs.GetString("Slot" + i.ToString("D2")));

        }
    }

    public void LoadInven()
    {
        for (int i = 0; i<Slots.Length; i++)
        {
            SlotOccu[i] = PlayerPrefs.GetInt("SlotOccu" + i.ToString("D2"));
            Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("Slot" + i.ToString("D2"));
            if (Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text != "")
            {
                Debug.Log("Slot " + i + " loaded with " + Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text);
                SlotNum--;
                SlotOccu[i] = 1;

                TextMeshProUGUI Text = Slots[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(); 

                Text.text = PlayerPrefs.GetString("Slot" + i.ToString("D2"));
                SlotNum--;
                SlotOccu[i] = 1;

                //클릭된 아이템의 이름을 받아와서, 월드에 그런 아이템이 있는지 확인 후 인벤에 추가. 월드에서는 비활성화
                foreach (KeyValuePair<string, Item> item in ItemMap)
                {
                    string iName = PlayerPrefs.GetString("Slot" + i.ToString("D2"));
                    if (item.Key == iName)
                    {  //여기 아래 두줄은 에디터상에서만 이용가능함
                        #if UNITY_EDITOR
                            string path = "Assets/CustomSprites/" + iName + ".png";
                            Sprite newSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                        #else
                            string path = "CustomSprites/" + iName;
                            Sprite newSprite = Resources.Load<Sprite>(path);
                        #endif
                        if (newSprite == null)
                        {
                            Debug.Log("Sprite not found at path: " + path);
                        }
                        else
                        {
                            // Slots[i].GetComponent<RawImage>().texture = newSprite;
                            // Slots[i].GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
                            Slots[i].GetComponent<Image>().sprite = newSprite;
                            Slots[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                            Slots[i].GetComponent<Image>().preserveAspect = true;
                            
                            ItemMap[iName].InInventory = true;
                        }
  
                    }
                }
            }

        }
    }


}

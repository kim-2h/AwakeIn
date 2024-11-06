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
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class InvenManager2 : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
                    { 
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
        

        if (NowMove == -1 || SlotOccu[NowMove] != 1)
        {
            return;
        }
        TempSlot.transform.position = Input.mousePosition;

        selectedObject.transform.position = Input.mousePosition;




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
        }
        else
        {

        }
        
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
   
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BedManager : MonoBehaviour, IPuzzle
{
    public GameObject ImageChange, InvenManager, GameFlowManager;
    public Button BtDrawer;
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    public TextMeshProUGUI Text;
    private Vector3 CameraPosition, DrawerPlace;
    public bool DrawerOpen = false;
    public bool CoverOpen = false;
    private Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();
    public Image[] Alphalists;

    public void StartPuzzle()
    {
        Debug.Log("Bed Puzzle Started");
        Debug.Log("Cover status: " + CoverOpen);
        canvas.gameObject.SetActive(true);
        BtDrawer.GetComponent<RectTransform>().anchoredPosition = new Vector2(DrawerPlace.x +230, DrawerPlace.y -80); 
        Key.SetActive(ItemMap["Clock_Key"].InInventory && !DrawerUnLocked && !ItemMap["Clock_Key"].IsUsed);
        CoverOpen = false;
        Key.GetComponent<RectTransform>().anchoredPosition = KeyPos;

        if (IsSolved)
        {
            Text.text = "nothing to do here";
        }
        else
        {
            Text.text = "this is Bed Puzzle";
        }
    }
    public void CoverClicked()
    {
        if (CoverOpen == false)
        {
            ImageChange.GetComponent<ImageChange>().SwitchSprite(
                canvas.gameObject.transform.Find("Cover").gameObject, "cover2");
            CoverOpen = true;
            Text.text = "there is something under the cover";
        }
        else if (CoverOpen)
        {
            ImageChange.GetComponent<ImageChange>().SwitchSprite(
            canvas.gameObject.transform.Find("Cover").gameObject, "cover1");
            Text.text = "I want to make it tidy";
            CoverOpen = false;
        }
    }
    public void DrawerClicked()
    {

        if (!DrawerOpen)
        {
            StartCoroutine(DrawerOpenAnimation());
            DrawerOpen = true;
            Text.text = "What's inside?";
        }
        else if (DrawerOpen)
        {
            StartCoroutine(DrawerOpenAnimation());
            //BtDrawer.transform.position = DrawerPlace;
            DrawerOpen = false;
            Text.text = "I'll close it for now";
        }
    }
    public void ExitPuzzle()
    {
        CoverOpen = false;
        
        if (canvas.gameObject.activeInHierarchy)
        {
            Debug.Log("Bed Puzzle Exit");
            Debug.Log("Cover status: " + CoverOpen);
            CoverOpen = false;
            DrawerOpen = false;
            canvas.gameObject.SetActive(false);
            Camera.main.gameObject.transform.position = CameraPosition;
            ImageChange.GetComponent<ImageChange>().SwitchSprite(
            canvas.gameObject.transform.Find("Cover").gameObject, "cover1");
            Text.text = "";
        }
        if (!IsSolved)
        {
            if (ItemMap["GasMask"].InInventory &&  ItemMap["Gear"].InInventory)
            {
                IsSolved = true;
                Text.text = "Nothing to do here";
            }
        }
    }
    void Start()
    {
        canvas.gameObject.SetActive(false);
        IsSolved = false;
        CameraPosition = Camera.main.transform.position;
        DrawerOpen = false;
        DrawerPlace = new Vector3(-92f, 2f, 0f);
        CoverOpen = false;
        BtDrawer.interactable = true;
        var CoverImage = canvas.gameObject.transform.Find("Cover").gameObject.GetComponent<Image>();
        CoverImage.alphaHitTestMinimumThreshold = 0.9f;

        foreach (var item in Alphalists)
        {
            item.alphaHitTestMinimumThreshold = 0.9f;
        }

        canvas.gameObject.transform.Find("Bed").gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.9f;
        ItemMap = GameFlowManager.GetComponent<GameFlowManager>().ItemMap;
    }

    IEnumerator DrawerOpenAnimation()
    {
        var Drawer = BtDrawer.transform.parent; 
        BtDrawer.interactable = false;
        var DrawerPos = DrawerOpen? new Vector3(DrawerPlace.x + 150, DrawerPlace.y-20, 0f) : DrawerPlace;
        var DrawerOpenPos = DrawerOpen? DrawerPlace : new Vector3(DrawerPos.x + 150, DrawerPos.y-20, 0f);
        var time = 0.5f;
        var elapsedTime = 0f;
        var DrawerRect = Drawer.GetComponent<RectTransform>();
        while (elapsedTime < time)
        {
            DrawerRect.anchoredPosition = Vector3.Lerp(DrawerPos, DrawerOpenPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            
        Drawer.GetComponent<RectTransform>().anchoredPosition = DrawerOpenPos;
        BtDrawer.interactable = true;
        //DrawerImage.raycastTarget = true;
    }
    public void DrawerItemClicked()
    {
        string Name = EventSystem.current.currentSelectedGameObject.name;
        if (Name == "GasMask" || Name == "Gear")
        {
            InvenManager.GetComponent<InvenManager>().ItemAdder(Name);
            //InvenManager.GetComponent<InvenManager>().ItemMap["GasMask"].InInventory = true;
            //InvenManager.GetComponent<InvenManager>().ItemMap["Gear"].InInventory = true;
        }
    }

    // void Awake()
    // {
    //     DrawerPlace = BtDrawer.transform.parent.position;
    //     Debug.Log(BtDrawer.transform.parent.position);
    // }

    //자물쇠 여는건 여기아래에다 적어놓음
    [Header("Drawer Managing")]
    public bool DrawerUnLocked = false;

    private Vector2 KeyPos = new Vector2(-600, 30);
    public GameObject Key, Lock;

    public void KeyOnDrag()
    {
        Key.transform.position = Input.mousePosition;
    }
    public void KeyOnDrop()
    {          Debug.Log("Key Rect : " + Key.GetComponent<RectTransform>().anchoredPosition + 
            "\nDrawer Rect : " + BtDrawer.GetComponent<RectTransform>().anchoredPosition + 
            "\nKey transformed position : " + Key.transform.position + 
            "\nDrawer transformed position : " + BtDrawer.transform.position);
        if (CoverOpen && Vector2.Distance(Key.GetComponent<RectTransform>().anchoredPosition, 
        BtDrawer.GetComponent<RectTransform>().anchoredPosition) < 100)
        {

            DrawerUnLocked = true;
            Key.SetActive(false);
            Lock.SetActive(false);
            ItemMap["Clock_Key"].IsUsed = true;
            InvenManager.GetComponent<InvenManager>().RemoveItem("Clock_Key");
        }
        else
        {
            // Reset the key to its original position using anchoredPosition
            Key.GetComponent<RectTransform>().anchoredPosition = KeyPos;
        }
    }




}

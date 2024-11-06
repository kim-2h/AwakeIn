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
    private Vector3 CameraPosition, DrawerPlace, ButtonPlace;
    private bool DrawerOpen = false;
    [SerializeField] public bool CoverOpen;
    private Dictionary<string, Item> ItemMap = new Dictionary<string, Item>();


    public void StartPuzzle()
    {
        Debug.Log("Bed Puzzle Started");
        canvas.gameObject.SetActive(true);
        //BtDrawer.GetComponent<RectTransform>().anchoredPosition = new Vector2(DrawerPlace.x + 247, DrawerPlace.y-69); 
        BtDrawer.GetComponent<RectTransform>().anchoredPosition = ButtonPlace;
        Key.SetActive(ItemMap["Clock_Key"].InInventory && !DrawerUnLocked && !ItemMap["Clock_Key"].IsUsed);
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
        Debug.Log("Cover Clicked");
        if (!CoverOpen)
        {
            ImageChange.GetComponent<ImageChange>().SwitchSprite(
                canvas.gameObject.transform.Find("Cover").gameObject, "Bedcover2");
            CoverOpen = true;
            Text.text = "there is something under the cover";
        }
        else if (CoverOpen)
        {
            ImageChange.GetComponent<ImageChange>().SwitchSprite(
            canvas.gameObject.transform.Find("Cover").gameObject, "Bedcover1");
            Text.text = "I want to make it tidy";
            CoverOpen = false;
        }
    }
    public void DrawerClicked()
    {

        if (!DrawerOpen && DrawerUnLocked)
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
        if (canvas.gameObject.activeInHierarchy)
        {
            Debug.Log("Bed Puzzle Exit");
            CoverOpen = false;
            DrawerOpen = false;
            BtDrawer.transform.parent.GetComponent<RectTransform>().anchoredPosition = DrawerPlace;
            
            
            Camera.main.gameObject.transform.position = CameraPosition;
            ImageChange.GetComponent<ImageChange>().SwitchSprite(
            canvas.gameObject.transform.Find("Cover").gameObject, "Bedcover1");
            Text.text = "";
            canvas.gameObject.SetActive(false);
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
        DrawerPlace = BtDrawer.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        ButtonPlace = BtDrawer.GetComponent<RectTransform>().anchoredPosition;
        IsSolved = false;
        CameraPosition = Camera.main.transform.position;
        DrawerOpen = false;
        //DrawerPlace = new Vector3(-97f, 2f, 0f);
        CoverOpen = false;
        BtDrawer.interactable = true;
        //var CoverImage = canvas.gameObject.transform.Find("Cover").gameObject.GetComponent<Image>();
        //CoverImage.alphaHitTestMinimumThreshold = 0.9f;
        //canvas.gameObject.transform.Find("Bed").gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.9f;
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
            
        DrawerRect.anchoredPosition = DrawerOpenPos;
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

    private Vector2 KeyPos = new Vector2(-670, 76);
    public GameObject Key, Lock;

    public void KeyOnDrag()
    {
        Key.transform.position = Input.mousePosition;
    }
    public void KeyOnDrop()
    {
        Debug.Log("KeyOnDrop, : " + Key.transform.position + "\n rect : " + Key.GetComponent<RectTransform>().anchoredPosition);
        
        var distance = Vector2.Distance(Key.GetComponent<RectTransform>().anchoredPosition, 
            BtDrawer.GetComponent<RectTransform>().anchoredPosition);
        if (CoverOpen && distance < 200)
        {
            Debug.Log("Key Unlocked : " + distance);

            Key.SetActive(false);
            Lock.SetActive(false);
            DrawerUnLocked = true;
            SoundManager.Instance.PlaySFX(2);
            ItemMap["Clock_Key"].IsUsed = true;
            InvenManager.GetComponent<InvenManager>().RemoveItem("Clock_Key");
        }
        else
        {
            // Reset the key to its original position using anchoredPosition
            Debug.Log("Key Reset : " + distance);
            Key.GetComponent<RectTransform>().anchoredPosition = KeyPos;
        }
    }




}

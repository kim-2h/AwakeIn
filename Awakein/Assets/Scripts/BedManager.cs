using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BedManager : MonoBehaviour, IPuzzle
{
    public GameObject ImageChange;
    public GameObject InvenManager;
    public Button BtDrawer;
    [SerializeField] public bool IsSolved { get; set; }
    public Canvas canvas;
    public TextMeshProUGUI Text;
    private Vector3 CameraPosition;
    private bool DrawerOpen = false;
    private bool CoverOpen = false;
    private Vector3 DrawerPlace;
    public void StartPuzzle()
    {
        Debug.Log("Bed Puzzle Started");
        canvas.gameObject.SetActive(true);
        BtDrawer.transform.parent.position = DrawerPlace; 
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
        if (!CoverOpen)
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
        if (canvas.gameObject.activeInHierarchy)
        {
            Debug.Log("Bed Puzzle Exit");
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
            if (InvenManager.GetComponent<InvenManager>().ItemList.Find(x => x.ItemName == "GasMask").InInventory &&
                InvenManager.GetComponent<InvenManager>().ItemList.Find(x => x.ItemName == "Gear").InInventory)
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
        DrawerPlace = BtDrawer.transform.parent.position;//new Vector3(8.8f, -79f, 0f);
        CoverOpen = false;
        BtDrawer.interactable = true;
        var CoverImage = canvas.gameObject.transform.Find("Cover").gameObject.GetComponent<Image>();
        CoverImage.alphaHitTestMinimumThreshold = 0.9f;
        canvas.gameObject.transform.Find("Bed").gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.9f;
    }

    IEnumerator DrawerOpenAnimation()
    {
        var Drawer = BtDrawer.transform.parent; 
        BtDrawer.interactable = false;
        var DrawerPos = DrawerOpen? new Vector3(DrawerPlace.x + 150, DrawerPlace.y-20, 0f) : DrawerPlace;
        var DrawerOpenPos = DrawerOpen? DrawerPlace : new Vector3(DrawerPos.x + 150, DrawerPos.y-20, 0f);
        var time = 0.5f;
        var elapsedTime = 0f;
        while (elapsedTime < time)
        {
            Drawer.transform.position = Vector3.Lerp(DrawerPos, DrawerOpenPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            
        Drawer.transform.position = DrawerOpenPos;
        BtDrawer.interactable = true;
        //DrawerImage.raycastTarget = true;
    }
    public void DrawerItemClicked()
    {
        string Name = EventSystem.current.currentSelectedGameObject.name;
        if (Name == "GasMask" || Name == "Gear")
        {
            InvenManager.GetComponent<InvenManager>().ItemAdder(Name);
            InvenManager.GetComponent<InvenManager>().ItemList.Find(x => x.ItemName == "GasMask").InInventory = true;
            InvenManager.GetComponent<InvenManager>().ItemList.Find(x => x.ItemName == "Gear").InInventory = true;
        }
    }

}

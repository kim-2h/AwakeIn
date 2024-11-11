using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClockManager : MonoBehaviour, IPuzzle
{
    public GameObject clock;
    public GameObject clockPopupCanvas;
    public ImageChange imageChanger;
    public GameObject ClockInPopUp;
    private RawImage hitClockImage;
    public Texture[] clockTextures;
    public TextMeshProUGUI popupText;
    private Vector3 initialCameraPosition;
    private bool isPopupActive = false;
    public Button backButton;
    public InvenManager invenManager;
    public GameObject Clock_Key; // 에너지 아이템 오브젝트 (Clock_Key에 backButton이 달려있다고 가정)
    private ChairPlaceManager ChairPlaceManager;

    [SerializeField] public bool IsSolved { get; set; }

    private int currentTextureIndex = 0;

    void Start()
    {
        clockPopupCanvas.SetActive(false);
        IsSolved = false;
        hitClockImage = ClockInPopUp.GetComponent<RawImage>();
        if (imageChanger != null)
        {
            imageChanger.ImgScene1.AddRange(clockTextures);
        }
        else
        {
            Debug.LogError("ImageChange component not assigned in ClockManager");
        }
        initialCameraPosition = Camera.main.transform.position;
        // if (backButton != null)
        // {
        //     backButton.onClick.AddListener(OnBackButtonPressed);
        //     backButton.gameObject.SetActive(false); // backButton 초기 비활성화
        // }
        // else
        // {
        //     Debug.LogError("BackButton is not assigned in ClockManager");
        // }
        if (Clock_Key != null)
        {
            Clock_Key.SetActive(false); // Clock_Key 초기 비활성화
        }
        else
        {
            Debug.LogError("Clock_Key object is not assigned in ClockManager");
        }
        ChairPlaceManager = GameObject.Find("Room1").transform.Find("ChairNPlaceholder").GetComponent<ChairPlaceManager>();
        //Debug.Log("ClockManager started. Initial state: Clock_Key and backButton are inactive.");
    }

    void Update()
    {
        // if (!isPopupActive && Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         if (hit.transform.gameObject == clock)
        //         {
        //             OpenPopup();
        //         }
        //         else if (hit.transform.CompareTag("item"))
        //         {
        //             HitClock_Key(hit.transform.gameObject);
        //         }
        //     }
        // }

        // if (isPopupActive && Input.GetMouseButtonDown(0))
        // {
        //     Vector2 localMousePosition = clockPopupCanvas.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        //     if (clockPopupCanvas.GetComponent<RectTransform>().rect.Contains(localMousePosition))
        //     {
        //         Vector2 hitLocalMousePosition = hitClockImage.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        //         if (hitClockImage.GetComponent<RectTransform>().rect.Contains(hitLocalMousePosition))
        //         {
        //             ChangeClockImage();
        //         }
        //     }
        // }
    }

    void OpenPopup()
    {
        clockPopupCanvas.SetActive(true);
        Clock_Key.SetActive(true);
        isPopupActive = true;
        Debug.Log("Popup opened.");
    }

    public void ChangeClockImage() //hitclock이랑 연결해줌 걍 
    {
        // currentTextureIndex = (currentTextureIndex + 1) % clockTextures.Length;
        // hitClockImage.texture = clockTextures[currentTextureIndex];
        // if (imageChanger != null && clock != null)
        // {
        //     imageChanger.SwitchImage(clock, currentTextureIndex);
        // } 
        if (imageChanger != null && clock != null)
        {
            string temp = hitClockImage.texture.name == "clockbackside" ? "clock" : "clockbackside";
            SoundManager.Instance.PlaySFX(10);
            imageChanger.SwitchImage(ClockInPopUp, temp);
        }
        else
        {
            Debug.LogError("ImageChange component or clock GameObject is not assigned");
        }

        // IsSolved = true; << 이게 여기있으면 안될거같은데 

        // 시계가 뒷면일 때만 에너지 아이템과 버튼을 활성화
        //bool isClockBackSide = currentTextureIndex != 0; // 0이 앞면이라고 가정
        bool isClockBackSide = hitClockImage.texture.name == "clockbackside"; 
        if (Clock_Key != null && !IsSolved)
        {
            Clock_Key.SetActive(true); // Clock_Key와 backButton을 함께 활성화/비활성화
            Clock_Key.GetComponent<Button>().interactable = isClockBackSide;
            Debug.Log($"Clock_Key set to {(isClockBackSide ? "active" : "inactive")}");
        }
        ///Debug.Log($"Clock image changed to index {currentTextureIndex}. IsClockBackSide: {isClockBackSide}");
    }
    public void KeyClicked()
    {
        IsSolved = true;
        invenManager.ItemAdder("Clock_Key");
    }

    public void ClosePopup()
    {
        clockPopupCanvas.SetActive(false);
        isPopupActive = false;
        Camera.main.transform.position = initialCameraPosition;
        popupText.text = "";
        if (Clock_Key != null)
        {
            Clock_Key.SetActive(false); // 팝업을 닫을 때 에너지 아이템과 버튼을 비활성화
            Debug.Log("Clock_Key set to inactive");
        }
        Debug.Log("Popup closed. Clock_Key and backButton are inactive.");
    }

    public void StartPuzzle()
    {
        if (ChairPlaceManager.ChairNow != ChairPlaceManager.ChairState.Clock)
        {
            invenManager.GameFlow.GetComponent<GameFlowManager>().CannotReach();
            return;
        }
        Debug.Log("Clock Puzzle Started");
        clockPopupCanvas.SetActive(true);
        imageChanger.SwitchImage(ClockInPopUp, "clock");
        if (IsSolved)
        {
            popupText.text = "There is nothing to do here";
        }
        else
        {
            popupText.text = "This is the Clock Puzzle";
        }

        // 퍼즐 시작 시 에너지 아이템과 버튼을 시계 뒷면일 때만 활성화
        bool isClockBackSide = hitClockImage.texture.name == "clockbackside";
        if (Clock_Key != null && !IsSolved)
        {
            Clock_Key.SetActive(true); 
            Clock_Key.GetComponent<Button>().interactable = isClockBackSide;
            // Clock_Key와 backButton을 함께 활성화/비활성화
            Debug.Log($"Clock_Key set to {(isClockBackSide ? "active" : "inactive")}");
        }
        Debug.Log($"Puzzle started. IsClockBackSide: {isClockBackSide}");
    }

    public void ExitPuzzle()
    {
        if (clockPopupCanvas.activeInHierarchy)
        {
            Debug.Log("Clock Puzzle Exit");
            ClosePopup();
        }
    }

    public void HitClock_Key(GameObject item)
    {
        Debug.Log("Item Clicked");
        if (invenManager != null)
        {
            invenManager.ItemAdder(item.name);
        }
        else
        {
            Debug.LogError("InvenManager is not assigned in ClockManager");
        }
        Destroy(item);
        popupText.text = "You got the item!";
    }

    void OnBackButtonPressed()
    {
        if (isPopupActive)
        {
            ClosePopup();
        }
        else
        {
            // BackButton이 HitClock_Key 역할을 수행
            Debug.Log("BackButton pressed");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Item"))
                {
                    HitClock_Key(hit.transform.gameObject);
                }
            }
        }
        if (Clock_Key != null)
        {
            Clock_Key.SetActive(false); // 작업 후 Clock_Key 비활성화 (backButton 포함)
        }
    }
}

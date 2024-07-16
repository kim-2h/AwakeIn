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
    public RawImage hitClockImage;
    public Texture[] clockTextures;
    public TextMeshProUGUI popupText;
    private Vector3 initialCameraPosition;
    private bool isPopupActive = false;
    public Button backButton;
    public InvenManager invenManager;
    public GameObject Energy; // 에너지 아이템 오브젝트 (Energy에 backButton이 달려있다고 가정)

    [SerializeField] public bool IsSolved { get; set; }

    private int currentTextureIndex = 0;

    void Start()
    {
        clockPopupCanvas.SetActive(false);
        IsSolved = false;
        if (imageChanger != null)
        {
            imageChanger.ImgScene1.AddRange(clockTextures);
        }
        else
        {
            Debug.LogError("ImageChange component not assigned in ClockManager");
        }
        initialCameraPosition = Camera.main.transform.position;
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonPressed);
            backButton.gameObject.SetActive(false); // backButton 초기 비활성화
        }
        else
        {
            Debug.LogError("BackButton is not assigned in ClockManager");
        }
        if (Energy != null)
        {
            Energy.SetActive(false); // Energy 초기 비활성화
        }
        else
        {
            Debug.LogError("Energy object is not assigned in ClockManager");
        }

        Debug.Log("ClockManager started. Initial state: Energy and backButton are inactive.");
    }

    void Update()
    {
        if (!isPopupActive && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == clock)
                {
                    OpenPopup();
                }
                else if (hit.transform.CompareTag("item"))
                {
                    HitEnergy(hit.transform.gameObject);
                }
            }
        }

        if (isPopupActive && Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePosition = clockPopupCanvas.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
            if (clockPopupCanvas.GetComponent<RectTransform>().rect.Contains(localMousePosition))
            {
                Vector2 hitLocalMousePosition = hitClockImage.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
                if (hitClockImage.GetComponent<RectTransform>().rect.Contains(hitLocalMousePosition))
                {
                    ChangeClockImage();
                }
            }
        }
    }

    void OpenPopup()
    {
        clockPopupCanvas.SetActive(true);
        isPopupActive = true;
        Debug.Log("Popup opened.");
    }

    void ChangeClockImage()
    {
        currentTextureIndex = (currentTextureIndex + 1) % clockTextures.Length;
        hitClockImage.texture = clockTextures[currentTextureIndex];
        if (imageChanger != null && clock != null)
        {
            imageChanger.SwitchImage(clock, currentTextureIndex);
        }
        else
        {
            Debug.LogError("ImageChange component or clock GameObject is not assigned");
        }

        IsSolved = true;

        // 시계가 뒷면일 때만 에너지 아이템과 버튼을 활성화
        bool isClockBackSide = currentTextureIndex != 0; // 0이 앞면이라고 가정
        if (Energy != null)
        {
            Energy.SetActive(isClockBackSide); // Energy와 backButton을 함께 활성화/비활성화
            Debug.Log($"Energy set to {(isClockBackSide ? "active" : "inactive")}");
        }
        Debug.Log($"Clock image changed to index {currentTextureIndex}. IsClockBackSide: {isClockBackSide}");
    }

    public void ClosePopup()
    {
        clockPopupCanvas.SetActive(false);
        isPopupActive = false;
        Camera.main.transform.position = initialCameraPosition;
        popupText.text = "";
        if (Energy != null)
        {
            Energy.SetActive(false); // 팝업을 닫을 때 에너지 아이템과 버튼을 비활성화
            Debug.Log("Energy set to inactive");
        }
        Debug.Log("Popup closed. Energy and backButton are inactive.");
    }

    public void StartPuzzle()
    {
        Debug.Log("Clock Puzzle Started");
        clockPopupCanvas.SetActive(true);
        if (IsSolved)
        {
            popupText.text = "There is nothing to do here";
        }
        else
        {
            popupText.text = "This is the Clock Puzzle";
        }

        // 퍼즐 시작 시 에너지 아이템과 버튼을 시계 뒷면일 때만 활성화
        bool isClockBackSide = currentTextureIndex != 0;
        if (Energy != null)
        {
            Energy.SetActive(isClockBackSide); // Energy와 backButton을 함께 활성화/비활성화
            Debug.Log($"Energy set to {(isClockBackSide ? "active" : "inactive")}");
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

    public void HitEnergy(GameObject item)
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
            // BackButton이 HitEnergy 역할을 수행
            Debug.Log("BackButton pressed");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("item"))
                {
                    HitEnergy(hit.transform.gameObject);
                }
            }
        }
        if (Energy != null)
        {
            Energy.SetActive(false); // 작업 후 Energy 비활성화 (backButton 포함)
        }
    }
}

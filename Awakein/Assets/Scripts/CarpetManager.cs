using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarpetManager : MonoBehaviour, IPuzzle
{
    public GameObject carpet;
    public GameObject carpetPopupCanvas;
    public ImageChange imageChanger;
    public RawImage hitCarpetImage;
    public Texture[] carpetTextures;
    public TextMeshProUGUI popupText;
    private Vector3 initialCameraPosition;
    private bool isPopupActive = false;

    [SerializeField] public bool IsSolved { get; set; }

    void Start()
    {
        carpetPopupCanvas.SetActive(false);
        IsSolved = false;
        imageChanger.ImgScene1.AddRange(carpetTextures);
        initialCameraPosition = Camera.main.transform.position;
    }

    void Update()
    {
        if (!isPopupActive && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == carpet)
                {
                    OpenPopup();
                }
            }
        }

        if (isPopupActive && Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePosition = carpetPopupCanvas.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
            if (carpetPopupCanvas.GetComponent<RectTransform>().rect.Contains(localMousePosition))
            {
                Vector2 hitLocalMousePosition = hitCarpetImage.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
                if (hitCarpetImage.GetComponent<RectTransform>().rect.Contains(hitLocalMousePosition))
                {
                    ChangeCarpetImage();
                }
            }
        }
    }

    void OpenPopup()
    {
        carpetPopupCanvas.SetActive(true);
        isPopupActive = true;
    }

    void ChangeCarpetImage()
    {
        int currentTextureIndex = System.Array.IndexOf(carpetTextures, hitCarpetImage.texture);
        int nextTextureIndex = (currentTextureIndex + 1) % carpetTextures.Length;
        hitCarpetImage.texture = carpetTextures[nextTextureIndex];
        imageChanger.SwitchImage(carpet, nextTextureIndex);
        IsSolved = true;
        popupText.text = "You changed the carpet!";
    }

    public void ClosePopup()
    {
        carpetPopupCanvas.SetActive(false);
        isPopupActive = false;
        Camera.main.transform.position = initialCameraPosition;
        popupText.text = "";
    }

    public void StartPuzzle()
    {
        Debug.Log("Carpet Puzzle Started");
        carpetPopupCanvas.SetActive(true);
        if (IsSolved)
        {
            popupText.text = "There is nothing to do here";
        }
        else
        {
            popupText.text = "This is the Carpet Puzzle";
        }
    }

    public void ExitPuzzle()
    {
        if (carpetPopupCanvas.activeInHierarchy)
        {
            Debug.Log("Carpet Puzzle Exit");
            ClosePopup();
        }
    }
}
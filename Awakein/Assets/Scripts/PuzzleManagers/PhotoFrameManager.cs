using System.Collections;
using UnityEngine;
using math = System.Math;

public class PhotoFrameManager : MonoBehaviour
{
    public GameObject Canvas, FrameFront, FrameBack, BackPlate, FrontPhoto;
    public InvenManager InvenManager;
    public GameFlowManager GameFlowManager;
    (Vector3, Vector3) pos0 = (new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    
    public void FrameClicked()
    {
        if (FrameFront.activeSelf)
        {
            FrameFront.SetActive(false);
            FrameBack.SetActive(true);
        }
        else if (FrameBack.activeSelf)
        {
            FrameFront.SetActive(true);
            FrameBack.SetActive(false);
        }

    }

    public void BackPlateClicked()
    {
        StartCoroutine(PlateMove());
    }

    public void ClosePopUp()
    {
        FrameFront.SetActive(true);
        FrameBack.SetActive(false);
        Canvas.SetActive(false);
        var rect = BackPlate.GetComponent<RectTransform>();
        rect.anchoredPosition = pos0.Item1;
        rect.rotation = Quaternion.Euler(pos0.Item2);
        if (InvenManager.ItemMap["FamilyPhoto"].InInventory && InvenManager.ItemMap["Photo_Note"].InInventory)
        {
            InvenManager.RemoveItem("PhotoFrame");
        }
    }

   public (Vector3, Vector3) pos1 = (new Vector3(-10, 2, 0), new Vector3(0, 0, 10));
    public (Vector3, Vector3) pos2 = (new Vector3(120, -2, 0), new Vector3(0, 0, -10));

    IEnumerator PlateMove()
    {        
        var time = 0.05f;
        var elapsedTime = 0f;
        var rect = BackPlate.GetComponent<RectTransform>();
        rect.anchoredPosition = pos0.Item1;
        rect.rotation = Quaternion.Euler(pos0.Item2);
        while (elapsedTime < time)
        {
            rect.anchoredPosition = Vector3.Lerp(pos0.Item1, pos1.Item1, elapsedTime / time);
            rect.rotation = Quaternion.Lerp(Quaternion.Euler(pos0.Item2), Quaternion.Euler(pos1.Item2), elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        elapsedTime = 0f;
        time = 0.25f;
        while (elapsedTime < time)
        {
            rect.anchoredPosition = Vector3.Lerp(pos1.Item1, pos2.Item1, math.Min(elapsedTime*1.5f  , time ) / time);
            rect.rotation = Quaternion.Lerp(Quaternion.Euler(pos1.Item2), Quaternion.Euler(pos2.Item2), elapsedTime/ time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
        }
    }
    public void PhotoClicked()
    {
        InvenManager.ItemAdder("FamilyPhoto");
        FrontPhoto.SetActive(false);
    }
    public void NoteClicked()
    {
        InvenManager.ItemAdder("Photo_Note");
    }

    void Start()
    {
        Debug.Log("PhotoFrameManager Start");
        var rect = BackPlate.GetComponent<RectTransform>();
        pos0 = (rect.anchoredPosition, rect.eulerAngles);
        Canvas.SetActive(true);
        FrameFront.SetActive(true);
        FrameBack.SetActive(false);
    }

    void Update()
    {
    }
}

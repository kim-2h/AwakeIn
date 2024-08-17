using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookShelfDrawerManager : MonoBehaviour, IPuzzle
{
    public Canvas canvas;
    public GameObject ClickBlock;
    public InvenManager InvenManager;
    public LockManagerCharacter LockCManager;
    public RawImage DrawerFront;
    private Vector3 CameraPosition;

    [SerializeField] public bool IsSolved { get; set; }

    public void StartPuzzle()
    {
        if (!IsSolved)
        {
            LockCManager.InitLock();
            canvas.gameObject.SetActive(true);
        }
        else if (IsSolved)
        {
            DrawerFront.gameObject.SetActive(false);
            ClickBlock.SetActive(true);
            canvas.gameObject.SetActive(true);
        }
    }

    public void ExitPuzzle()
    {
        canvas.gameObject.SetActive(false);
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    public void LockSolved()
    {
        IsSolved = true;
        ClickBlock.SetActive(true);
        StartCoroutine(OpenDrawer());
    }



    void Start()
    {
        IsSolved = false;
        CameraPosition = Camera.main.transform.position;
        canvas.gameObject.SetActive(false);
        ClickBlock.SetActive(false);
    }

    public void FishFoodClicked()
    {
        InvenManager.ItemAdder("FishFood");
    }
    public void ChairClicked()
    {
        InvenManager.ItemAdder("Chair");
    }   

    IEnumerator OpenDrawer()
    {
        Debug.Log("서랍 열리는중...");
        float time = 2f;
        float now = 0f;
        Color color = DrawerFront.color;
        while (time > now)
        {
            now += Time.deltaTime;
            DrawerFront.color = Color.Lerp(color, new Color(color.a, color.g, color.b, 0), now/time);
            yield return null;
        }
        DrawerFront.GetComponent<RawImage>().raycastTarget = false;
        DrawerFront.gameObject.SetActive(false);
    }

}

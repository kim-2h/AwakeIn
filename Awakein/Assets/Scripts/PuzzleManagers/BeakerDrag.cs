using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeakerDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public ChemicalsManager ChemicalsManager;
    public GameObject TempParent, Origin;
    public int Idx;

    private Color Yellow, Blue, Green;

public void OnBeginDrag(PointerEventData eventData)
{
    ChemicalsManager.DraggedBeaker = this.gameObject;

    // 강제로 현재 오브젝트를 선택 상태로 설정
    EventSystem.current.SetSelectedGameObject(this.gameObject);

    // Debug.Log에 null 체크 추가
    if (EventSystem.current.currentSelectedGameObject != null)
    {
        Debug.Log("Clicked : " + EventSystem.current.currentSelectedGameObject.name);
    }
    else
    {
        Debug.Log("No object selected");
    }

    string Name = this.name;
    switch (Name)
    {
        case "Beaker80":
            Idx = 0;
            break;
        case "Beaker40":
            Idx = 2;
            break;
        case "Beaker30":
            Idx = 1;
            break;
        default:
            break;
    }
    this.transform.rotation = Quaternion.Euler(0, 0, 30);
    Debug.Log("Beaker Drag Start");
    this.transform.SetParent(TempParent.transform);
}

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform Beaker = this.GetComponent<RectTransform>();
        var IMG2 = ChemicalsManager.Sliders[1].transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        var IMG = ChemicalsManager.Sliders[0].transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        var Cslider = ChemicalsManager.Cylinder.transform.GetChild(0).GetComponent<Slider>();

        if (Vector3.Distance(ChemicalsManager.CylinderPos, Beaker.anchoredPosition) < 100f)
        {
            if (ChemicalsManager.Amount[1] == 60 && IMG2.color == Yellow)
            {
                Debug.Log("Beaker over : Cylinder");
                //ChemicalsManager.Calc(Idx, 0);
                ChemicalsManager.BottleB.GetComponent<Button>().interactable = true;
                ChemicalsManager.BottleA.GetComponent<Button>().interactable = false;
                Cslider.value += 60f;
                var Jobdone =  Cslider.value == 110 ? true : false;
                ChemicalsManager.InitSolution();
                if (Jobdone)
                {
                    IMG.color = Green;
                    Cslider.value = 110;
                    ChemicalsManager.Amount[0] = 110;
                    ChemicalsManager.PuzzleSolved();
                }
                else
                {
                    IMG.color = Yellow;
                    SoundManager.Instance.PlaySFX(2);
                    Cslider.value = 60; 
                    ChemicalsManager.Amount[0] = 60;
                }

            }
            else if (ChemicalsManager.Amount[1] == 50 && IMG2.color == Blue)
            {
                 
                Debug.Log("Beaker over : Cylinder");
                //ChemicalsManager.Calc(Idx, 0);
                ChemicalsManager.BottleB.GetComponent<Button>().interactable = false;
                ChemicalsManager.BottleA.GetComponent<Button>().interactable = true;
                Cslider.value += 50f;
                var Jobdone =  Cslider.value == 110 ? true : false;
                ChemicalsManager.InitSolution();
                if (Jobdone)
                {
                    IMG.color = Green;
                    Cslider.value = 110;
                    ChemicalsManager.Amount[0] = 110;
                    ChemicalsManager.PuzzleSolved();
                }
                else
                {
                    IMG.color = Blue;
                    Cslider.value = 50;
                    ChemicalsManager.Amount[0] = 50;
                }
                
            }
        }

        else if (Vector3.Distance(ChemicalsManager.BeakerPos[(Idx+1)%3], Beaker.anchoredPosition) < 100f)
        {
            Debug.Log("Beaker over : " + ChemicalsManager.BeakerPos[(Idx+1)%3]);
            ChemicalsManager.Calc(Idx, (Idx+1)%3);
           
            
        }
        else if (Vector3.Distance(ChemicalsManager.BeakerPos[(Idx+2)%3], Beaker.anchoredPosition) < 100f)
        {
            Debug.Log("Beaker over : " + ChemicalsManager.BeakerPos[(Idx+2)%3]);
            ChemicalsManager.Calc(Idx, (Idx+2)%3);
            
        }
        else 
        {
            
        }
        Beaker.GetComponent<RectTransform>().anchoredPosition = ChemicalsManager.BeakerPos[Idx];
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.transform.SetParent(Origin.transform);
    }

    void Start()
    {
        Yellow = ChemicalsManager.Yellow;
        Blue = ChemicalsManager.Blue;
        Green = ChemicalsManager.Green;
        
    }

}

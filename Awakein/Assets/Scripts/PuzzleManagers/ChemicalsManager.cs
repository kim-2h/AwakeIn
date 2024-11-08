using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChemicalsManager : MonoBehaviour, IPuzzle
{
    public bool IsSolved { get; set; }


    public InvenManager InvenManager;
    public GameFlowManager GameFlowManager;
    public Canvas canvas;
    public GameObject BottleA, BottleB, BeakerL, BeakerS, BeakerM, GasMask, Cylinder, GasMasKPOV;

    public float[] Amount = new float[4] { 0f, 0f, 0f, 0f};
    private float[] Maxes = new float[4] { 100f, 80f, 30f, 40f};
    public Slider[] Sliders;
    public Vector3[] BeakerPos = new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero};
    public Vector3 CylinderPos = Vector3.zero;
    //순서대로 비커 큰, 작은, 중간

    private Vector3 CameraPosition;
public Color32 Green = new Color32(92, 143, 66, 150);
public Color32 Blue = new Color32(60, 70, 200, 200);
public Color32 Yellow = new Color32(230, 220, 80, 200);

    public void StartPuzzle()
    {
        canvas.gameObject.SetActive(true);

        if (!IsSolved)
        {
            GasMasKPOV.SetActive(false);
            GasMask.SetActive(InvenManager.ItemMap["GasMask"].InInventory ? true : false);

            
            BottleA.SetActive(InvenManager.ItemMap["BottleA"].InInventory ? true : false);
            BottleB.SetActive(InvenManager.ItemMap["BottleB"].InInventory ? true : false);
            GasMask.GetComponent<Button>().interactable = BottleA.activeSelf && BottleB.activeSelf;

            BottleA.GetComponent<Button>().interactable = false;
            BottleB.GetComponent<Button>().interactable = false;

            BeakerL.transform.GetChild(1).gameObject.SetActive(false);
            BeakerM.transform.GetChild(1).gameObject.SetActive(false);
            BeakerS.transform.GetChild(1).gameObject.SetActive(false);
            Cylinder.transform.GetChild(1).gameObject.SetActive(false);

            Cylinder.GetComponent<Button>().interactable = false;

            InitSolution();
        }
        else
        {
            GasMasKPOV.SetActive(false);
            GasMask.GetComponent<Button>().interactable = false;
            Cylinder.GetComponent<Button>().interactable = true;
        }

    }

    public void ExitPuzzle()
    {
        canvas.gameObject.SetActive(false);
        Camera.main.gameObject.transform.position = CameraPosition;
    }

    public void GetPoison()
    {
        Cylinder.transform.name = "PoisonGas";
        
        InvenManager.RemoveItem("GasMask");
        InvenManager.RemoveItem("BottleA");
        InvenManager.RemoveItem("BottleB");
        InvenManager.ItemAdder("PoisonGas");
    }

    void Start()
    {
        canvas.gameObject.SetActive(false);

        // BottleA.SetActive(false);
        // BottleB.SetActive(false);
        // GasMask.SetActive(false);
        // GasMasKPOV.SetActive(false);

        // BeakerL.transform.GetChild(1).gameObject.SetActive(false);
        // BeakerM.transform.GetChild(1).gameObject.SetActive(false);
        // BeakerS.transform.GetChild(1).gameObject.SetActive(false);
        // Cylinder.transform.GetChild(1).gameObject.SetActive(false);
        Sliders = new Slider[4] { Cylinder.transform.GetChild(0).GetComponent<Slider>(),
        BeakerL.transform.GetChild(0).GetComponent<Slider>(), 
        BeakerS.transform.GetChild(0).GetComponent<Slider>(), 
        BeakerM.transform.GetChild(0).GetComponent<Slider>() };

        CameraPosition = Camera.main.gameObject.transform.position;
        BeakerPos[0] = new Vector3(-110f, -50f, 0f);
        BeakerPos[2] = new Vector3(350f, -50f, 0f);
        BeakerPos[1] = new Vector3(120f, -80f, 0f);
        CylinderPos = new Vector3(-350f, -20f, 0f);

        BeakerL.GetComponent<RectTransform>().anchoredPosition = BeakerPos[0];
        BeakerS.GetComponent<RectTransform>().anchoredPosition = BeakerPos[1];
        BeakerM.GetComponent<RectTransform>().anchoredPosition = BeakerPos[2];
        Cylinder.GetComponent<RectTransform>().anchoredPosition = CylinderPos;

    }

    public void GasMaskClick()
    {
        Debug.Log("Clicked : " + EventSystem.current.currentSelectedGameObject.name);
        BeakerL.transform.GetChild(1).gameObject.SetActive(true);
        BeakerM.transform.GetChild(1).gameObject.SetActive(true);
        BeakerS.transform.GetChild(1).gameObject.SetActive(true);
        //Cylinder.transform.GetChild(1).gameObject.SetActive(true);

        GasMasKPOV.SetActive(true);
        GasMask.SetActive(false);

        BottleA.GetComponent<Button>().interactable = true;
        BottleB.GetComponent<Button>().interactable = true;
    }

    public void BottleAClicked()
    {
        BottleA.GetComponent<Button>().interactable = false;
        BottleB.GetComponent<Button>().interactable = false;
       // Debug.Log("Clicked : " + EventSystem.current.currentSelectedGameObject.name);
    

        BeakerL.transform.GetChild(0).GetComponent<Slider>().value = 80f;
        // BeakerL.transform.GetChild(0).GetComponent<Slider>().transform.GetChild(1).transform.GetChild(0).
        // GetComponent<Image>().color = Color.magenta;

        for (int i = 1; i < 4; i++)
        {
            Sliders[i].transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = this.Yellow;
        }
        Amount[1] = 80f;
    }

    public void BottleBClicked()
    {
        BottleA.GetComponent<Button>().interactable = false;
        BottleB.GetComponent<Button>().interactable = false;
        //Debug.Log("Clicked : " + EventSystem.current.currentSelectedGameObject.name);

        BeakerL.transform.GetChild(0).GetComponent<Slider>().value = 70f;
        // BeakerL.transform.GetChild(0).GetComponent<Slider>().transform.GetChild(1).transform.GetChild(0).
        // GetComponent<Image>().color = Color.cyan;

        for (int i = 1; i < 4; i++)
        {
            Sliders[i].transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = this.Blue;
        }
        Amount[1] = 70f;
    }

    public void PuzzleSolved()
    {
        IsSolved = true;

        BottleA.GetComponent<Button>().interactable = false;
        BottleB.GetComponent<Button>().interactable = false;

        BeakerL.GetComponent<RawImage>().raycastTarget = false;  
        BeakerM.GetComponent<RawImage>().raycastTarget = false;  
        BeakerS.GetComponent<RawImage>().raycastTarget = false;  

        Cylinder.GetComponent<Button>().interactable = true;
    }

    public void InitSolution()
    {
        BeakerL.transform.GetChild(0).GetComponent<Slider>().value = 0;
        BeakerS.transform.GetChild(0).GetComponent<Slider>().value = 0;
        BeakerM.transform.GetChild(0).GetComponent<Slider>().value = 0;

        Cylinder.transform.GetChild(0).GetComponent<Slider>().value = 0;

        for (int i = 0; i < Amount.Length; i++)
        {
            Amount[i] = 0f;
        }
    }
    public int Idx = 0;
    public GameObject DraggedBeaker;

    public void Calc(int a, int b)
    {
        if (Amount[a+1] == 0)
        {
            return;
        }
        else
        {
            float Water = Maxes[b+1] - Amount[b+1];
            Water = Water > Amount[a+1] ? Amount[a+1] : Water;
            Amount[a+1] -= Water;
            Sliders[a+1].value = Amount[a+1];
            Amount[b+1] += Water;
            Sliders[b+1].value = Amount[b+1];
        }

    }

}

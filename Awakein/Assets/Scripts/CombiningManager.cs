using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class CombiningManager : MonoBehaviour
{/*combiningcanvas에 오르골부품 움직일 애들이랑 그 자리 까지 다 세팅하기*/
    public Canvas canvas;
    public Texture[] images;
    bool IsCombined=false;
    public bool ClickNotDrag = true;
    Button[] buttons;
    public GameObject[] gameObjects;
    public GameObject[] ResultItem;
    InvenManager invenManager;
    void Start()
    {
        invenManager=GetComponent<InvenManager>();
        buttons = gameObject.GetComponentsInChildren<Button>();
        // foreach (Button button in buttons)
        // {
        //     button.onClick.AddListener(() => OnButtonClicked());
        // }
    }

    public void OnButtonClicked()
    {
        if (!ClickNotDrag)
        {
            return;
        }
        Debug.Log("Button clicked for matching");
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        RawImage rawImage = button.GetComponent<RawImage>();

        canvas.transform.GetChild(0).gameObject.SetActive(true);
        canvas.transform.GetChild(1).gameObject.SetActive(true);
        canvas.transform.GetChild(2).gameObject.SetActive(true);

        foreach (GameObject obj in ResultItem)
        {
            obj.SetActive(false);
        }
        
        if (rawImage.texture.name == images[0].name || rawImage.texture.name == images[1].name)//드라이버 부분
        {
            Debug.Log("Image Right: Driver");
            canvas.gameObject.SetActive(true);
            canvas.transform.GetChild(3).gameObject.SetActive(true);
            //Debug.Log("Setactive2" + canvas.transform.GetChild(2).gameObject.activeSelf);
            canvas.transform.GetChild(4).gameObject.SetActive(true);
            //Debug.Log("Setactive3" + canvas.transform.GetChild(3).gameObject.activeSelf);
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }
            InvokeMatch();
            ResultItem[1].SetActive(false);
            if (invenManager.ItemMap["DriverHandle"].InInventory && !invenManager.ItemMap["DriverHandle"].IsUsed)
            {
                gameObjects[0].SetActive(true);
                gameObjects[0].GetComponent<CombiningDragHandler>().PlaceHolder.SetActive(true);
            }
            if (invenManager.ItemMap["DriverStick"].InInventory && !invenManager.ItemMap["DriverStick"].IsUsed)
            {
                gameObjects[1].SetActive(true);
                gameObjects[1].GetComponent<CombiningDragHandler>().PlaceHolder.SetActive(true);
            }
            // foreach (GameObject obj in gameObjects)
            // {
            //     if (obj.name == images[0].name  || obj.name == images[1].name )
            //     {
            //         obj.SetActive(true);
            //     }
            //     else
            //     {
            //         obj.SetActive(false);
            //     }
            // }
       
            // if (rawImage.texture == images[0]){//handle부분
            //     canvas.transform.GetChild(4).gameObject.SetActive(true); //하이어라키 순서를 바꾸면 ㅈ댑니다.
            //     canvas.transform.GetChild(5).gameObject.SetActive(false);
            //    foreach (Button but in buttons){
            //       if (but.GetComponent<RawImage>().texture==images[1]){
            //             canvas.transform.GetChild(5).gameObject.SetActive(true);

            //       }
            //    }
            // }
            // if (rawImage.texture == images[1]){//stick 부분
            //     canvas.transform.GetChild(5).gameObject.SetActive(true);
            //     canvas.transform.GetChild(4).gameObject.SetActive(false);
            //    foreach (Button but in buttons){
            //       if (but.GetComponent<RawImage>().texture==images[0]){
            //         canvas.transform.GetChild(4).gameObject.SetActive(true);
            //       }
            //    }
            // }
            
        }//비행기 부분 추가해야 함
        else if (rawImage.texture.name == images[2].name || rawImage.texture.name == images[3].name)
        {
         Debug.Log("Image Right: Orgel");
            canvas.gameObject.SetActive(true);
            canvas.transform.GetChild(9).gameObject.SetActive(true);
            canvas.transform.GetChild(10).gameObject.SetActive(true);
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }
            InvokeMatch();
            ResultItem[0].SetActive(false);
            if (invenManager.ItemMap["Orgel"].InInventory && !invenManager.ItemMap["Orgel"].IsUsed)
            {
                gameObjects[2].SetActive(true);
                gameObjects[2].GetComponent<CombiningDragHandler>().PlaceHolder.SetActive(true);
            }
            if (invenManager.ItemMap["OrgelBody"].InInventory && !invenManager.ItemMap["OrgelBody"].IsUsed)
            {
                gameObjects[3].SetActive(true);
                gameObjects[3].GetComponent<CombiningDragHandler>().PlaceHolder.SetActive(true);
            }
            // if (rawImage.texture == images[2]){//
            //     canvas.transform.GetChild(6).gameObject.SetActive(true); 
            //     canvas.transform.GetChild(7).gameObject.SetActive(false);
            //    foreach (Button but in buttons){
            //       if (but.GetComponent<RawImage>().texture==images[3])
            //       {
            //         canvas.transform.GetChild(7).gameObject.SetActive(true);
            //       }
            //    }
            // }
            // if (rawImage.texture == images[3]){
            //     canvas.transform.GetChild(6).gameObject.SetActive(true);
            //     canvas.transform.GetChild(7).gameObject.SetActive(false);
            //    foreach (Button but in buttons){
            //       if (but.GetComponent<RawImage>().texture==images[2])
            //       {
            //          canvas.transform.GetChild(6).gameObject.SetActive(true);
            //       }
            //    }
            // }
        }
       
    }
    public void Exit()
    {
        canvas.gameObject.SetActive(false);
        canvas.transform.GetChild(3).gameObject.SetActive(false);
        canvas.transform.GetChild(4).gameObject.SetActive(false); 
        canvas.transform.GetChild(9).gameObject.SetActive(false);
        canvas.transform.GetChild(10).gameObject.SetActive(false); 
    }

    public void InvokeMatch()
    {
        if (gameObjects[0].GetComponent<CombiningDragHandler>().isMatched && 
            gameObjects[1].GetComponent<CombiningDragHandler>().isMatched && !invenManager.ItemMap["Driver"].InInventory
            && !invenManager.ItemMap["Driver"].IsUsed && canvas.transform.GetChild(3).gameObject.activeSelf)
        {
            ResultItem[0].SetActive(true);
        }
        else if (gameObjects[2].GetComponent<CombiningDragHandler>().isMatched && 
            gameObjects[3].GetComponent<CombiningDragHandler>().isMatched &&
            !invenManager.ItemMap["OrgelWhole"].InInventory && !invenManager.ItemMap["OrgelWhole"].IsUsed 
            && canvas.transform.GetChild(9).gameObject.activeSelf)
        {
            ResultItem[1].SetActive(true);
        }
    }

//     void Update() 
//     {
//     if (gameObjects[0].GetComponent<CombiningDragHandler>().isMatched && 
//         gameObjects[1].GetComponent<CombiningDragHandler>().isMatched){
//         IsCombined=true;
//         ResultItem.SetActive(true);
        
//     }//여기에 움직일 비행기 부품들 조건 복붙해야 함 resultItem도 list로 만들어야 함
//    } 업 데 이 트 문 쓰 지 맙 시 다. 업데이트문은 존재만으로 리소스를 잡아먹고 게임을 느려지게한다.

   public void DriverAdd(){
        invenManager.ItemMap["DriverHandle"].IsUsed = true;
        invenManager.RemoveItem("DriverHandle");

        invenManager.ItemMap["DriverStick"].IsUsed = true;
        invenManager.RemoveItem("DriverStick");
        invenManager.ItemAdder("Driver");   
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        canvas.transform.GetChild(3).gameObject.SetActive(false);        
   }

   public void OrgelAdd(){
        invenManager.ItemMap["Orgel"].IsUsed = true;
        invenManager.RemoveItem("Orgel");
        invenManager.ItemMap["OrgelBody"].IsUsed = true;
        invenManager.RemoveItem("OrgelBody");
        invenManager.ItemAdder("OrgelWhole");
        canvas.transform.GetChild(6).gameObject.SetActive(false);
        canvas.transform.GetChild(7).gameObject.SetActive(false); 
     //여기서 부품들 사라져야 함
   }
}

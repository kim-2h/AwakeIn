using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombiningManager : MonoBehaviour
{/*combiningcanvas에 오르골부품 움직일 애들이랑 그 자리 까지 다 세팅하기*/
    public Canvas canvas;
    public Texture[] images;
    bool IsCombined=false;
    Button[] buttons;
    public GameObject[] gameObjects;
    public GameObject ResultItem;
    InvenManager invenManager;
    void Start()
    {
        invenManager=GetComponent<InvenManager>();
         buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }

    public void OnButtonClicked(Button button)
    {
        Debug.Log("Button clicked");

        RawImage rawImage = button.GetComponent<RawImage>();
       
        
        if ((rawImage.texture == images[0] || rawImage.texture == images[1]))//드라이버 부분
        {
            Debug.Log("Image Right");
            canvas.gameObject.SetActive(true);
       
            if (rawImage.texture == images[0]){//handle부분
                canvas.transform.GetChild(4).gameObject.SetActive(true);
                canvas.transform.GetChild(5).gameObject.SetActive(false);
               foreach (Button but in buttons){
                
                  if (but.GetComponent<RawImage>().texture==images[1]){
                        canvas.transform.GetChild(5).gameObject.SetActive(true);

                  }
               }
            }
            if (rawImage.texture == images[1]){//stick 부분
                canvas.transform.GetChild(5).gameObject.SetActive(true);
                canvas.transform.GetChild(4).gameObject.SetActive(false);
               foreach (Button but in buttons){
                  if (but.GetComponent<RawImage>().texture==images[0]){
                              canvas.transform.GetChild(4).gameObject.SetActive(true);
                  }
               }
            }
            
        }//비행기 부분 추가해야 함
       
    }
    public void Exit(){
        canvas.gameObject.SetActive(false);
    }
    void Update(){
    if (gameObjects[0].GetComponent<CombiningDragHandler>().isMatched&&gameObjects[1].GetComponent<CombiningDragHandler>().isMatched){
        IsCombined=true;
        ResultItem.SetActive(true);
        
    }//여기에 움직일 비행기 부품들 조건 복붙해야 함 resultItem도 list로 만들어야 함
   }
   public void DriverAdd(){
    invenManager.ItemMap["DriverHandle"].IsUsed = true;
        invenManager.RemoveItem("DriverHandle");

        invenManager.ItemMap["DriverStick"].IsUsed = true;
        invenManager.RemoveItem("DriverStick");
     invenManager.ItemAdder("Driver");
    
          
        
   }
   public void OregolAdd(){
     invenManager.ItemAdder("WholeOregol");
     //여기서 부품들 사라져야 함
   }
}

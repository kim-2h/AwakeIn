using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bookcover : MonoBehaviour
{
    //북 터치하면 canvas 열고 열리면 시작하기 
   
    //[SerializeField]
   public GameObject bookcanvas;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       
        cam = Camera.main;
    }
     
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "bookcard")
            {
                bookcanvas.SetActive(true);
                
            }//이현이 꺼 쓰고 싶었지만 코드 public으로 바꿔야 해서 이렇게 함 나중에 수정하면 가능
    }
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CameraMoving2 : MonoBehaviour
{   
    public DialogueManager dialogueManager;
    public GameFlowManager gameFlowManager;
    public GameObject InvenManager;
    public Camera cam; // 확대할 카메라 이름
    public float zoomSpeed = 0.5f; // 확대 속도

    private Vector3 initialPosition; // 초기 카메라 위치
    private bool isFirstClick = true; // 첫 번째 클릭 여부
    private bool isZooming = false; // 확대 중 여부
   Rect ClickArea;
    void Start()
    {
        //ClickArea = new Rect(5, 5, 1920-10, 1080-10);
        SetClickArea();
        initialPosition = cam.transform.position; // 초기 위치 저장
        Cursor.lockState = CursorLockMode.Confined; // 마우스 커서 고정. ctrl+p로 게임 종료가능
        //InvenManager = GameObject.Find("InvenCanvas").transform.Find("InvenBG").gameObject;
      
    }
    void SetClickArea()
    {
        float padding = 5f; // 경계로부터의 패딩 (픽셀 단위)
        float aspectRatio = 1920f / 1080f; // 16:9 비율

        // 가로를 기준으로 세로 높이를 설정하여 비율 유지
        float width = Screen.width - padding * 2;
        float height = width / aspectRatio;

        // 클릭 영역이 화면을 벗어나지 않도록 높이를 제한
        if (height > (Screen.height - padding * 2))
        {
            height = Screen.height - padding * 2;
            width = height * aspectRatio;
        }

        // 클릭 영역을 화면 중앙에 배치
        float x = (Screen.width - width) / 2;
        float y = (Screen.height - height) / 2;

        ClickArea = new Rect(x, y, width, height);

        Debug.Log($"ClickArea 설정됨: {ClickArea}");
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!ClickArea.Contains(Input.mousePosition))
            {
                return;
            }
        }

        if (Input.GetMouseButtonDown(0) && !isZooming && !EventSystem.current.IsPointerOverGameObject()) // 만약 왼쪽 클릭이 발생하고 확대 중이 아니면
        {

            if (cam.transform.position == initialPosition) // 초기 위치에서 클릭 시 isFirstClick을 true로 설정
            {                                          //초기위치로 돌아가는 함수가 다른곳에 있을때를 대비
                isFirstClick = true; 
            }

            
            // 레이캐스트를 수행하여 클릭된 객체를 결정
            RaycastHit hit;
 

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10f);
            if (!isFirstClick && !isZooming) // 확대된 상태고 줌인 중이 아니면 클릭 시 초기 위치로 되돌림
            {
                cam.transform.position = initialPosition;
                isFirstClick = true;
            }
            else if (EventSystem.current.IsPointerOverGameObject() == false && (Physics.Raycast(ray, out hit))) // 레이캐스트 성공 시
            {
                string dialogue = "";

                dialogue = gameFlowManager.ReturnDialogue(hit.transform.name);
                
                if (dialogue != null && dialogue != "" && dialogueManager.IsDialoguePlaying == false)
                {
                    dialogueManager.CallRoutine(dialogue);
                }
                if (hit.transform.tag == "Item") 
                {
                    Debug.Log("Item Clicked: " + hit.transform.name);
                    ItemClicked(hit.transform.name);
                }    
                else if (isFirstClick && hit.transform.name != "PlantPot") // 클릭된 객체가 Puzzle 태그를 가지고 있고, 처음 클릭이면
                {
                    HitSTH(hit);
     
                    isFirstClick = false; 
                }
                else
                {
                    // 처음 상태로 되돌림
                    cam.transform.position = initialPosition;
                    isFirstClick = true;
                }
                if (hit.transform.name == "PlantPot")
                {
                    hit.transform.gameObject.GetComponent<PlantPotMovement>().PlantPotClicked();
                }

            }
        }

    }

    private void HitSTH(RaycastHit hit)
    {
        
                   Debug.Log("Puzzle Clicked: " + hit.transform.name);
                    StartCoroutine(ZoomCoroutine(hit.point)); // 클릭된 객체 위치를 기준으로 확대 시작
                    
                    IPuzzle puzzle = hit.transform.gameObject.GetComponent<IPuzzle>();
                    if (puzzle != null)
                    {
                        StartCoroutine(StartPuzzleAfterZoom(puzzle));
                    }
                    
                    else if (hit.transform.tag == "Puzzle" && hit.transform.gameObject.transform.parent.GetComponent<IPuzzle>() != null)
                    {
                        puzzle = hit.transform.gameObject.transform.parent.GetComponent<IPuzzle>();
                        StartCoroutine(StartPuzzleAfterZoom(puzzle));
                    }
                    else if (hit.transform.name == "DoorBack")
                    {
                        gameFlowManager.Ending();
                    }
                    else
                    {
                        Debug.Log("No puzzle script found");
                    }
                    isFirstClick = false;
    }

    private IEnumerator StartPuzzleAfterZoom(IPuzzle puzzle)
    {
        yield return new WaitForSeconds(0.1f); // 1초 대기
        puzzle.StartPuzzle(); // 퍼즐 시작
    }



    public void ItemClicked(string iName)
    {
        Debug.Log(iName); 
  
        InvenManager.GetComponent<InvenManager>().ItemAdder(iName); 
    }
    public float ZoomDistance = 100f;

    IEnumerator WaitAWhile(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator ZoomCoroutine(Vector3 targetPosition)
    {
        isZooming = true; // 확대 중 플래그 설정
        float elapsedTime = 0f;
        Vector3 startPosition = cam.transform.position;


        while (elapsedTime < 1.5f) // 2초 동안 확대
        {
            elapsedTime += Time.deltaTime * zoomSpeed; 
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 2f); // 초기 위치에서 목표 위치까지 Lerp

            yield return null; // 다음 프레임까지 대기
        }

        isZooming = false; // 확대 종료 후 플래그 해제
    }

}
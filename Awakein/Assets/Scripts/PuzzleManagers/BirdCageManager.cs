using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BirdCageManager : MonoBehaviour, IPuzzle
{
    public bool IsSolved { get; set; }
    public Canvas canvas;
    public InvenManager InvenManager;
    public GameObject Bird, Poison, DeadBird;
    private RectTransform PoisonRect;
    private Vector3 CameraPosition, BirdPosition, DispenserPos;
    private bool RutineOn = false;
    public Vector3 PoisonPos = new Vector3(-740f, -280f, 0f);

    public void StartPuzzle()
    {
        canvas.gameObject.SetActive(true);
        PoisonRect = Poison.GetComponent<RectTransform>();
        PoisonRect.anchoredPosition = PoisonPos;
        if (!IsSolved)
        {
            Bird.GetComponent<RectTransform>().anchoredPosition = BirdPosition;
            DeadBird.SetActive(false);
            
            RutineOn = true;
            StartCoroutine(BirdMove(new Vector3(BirdPosition.x-200f, BirdPosition.y, BirdPosition.z), 
            BirdPosition, 25f, 5f, 10f, 10f));

            Poison.SetActive(InvenManager.ItemMap["PoisonGas"].InInventory ? true : false);
        }
        else
        {
            Poison.SetActive(true);
            
            Bird.SetActive(false);
            DeadBird.SetActive(true);
        }

    }
    public void ExitPuzzle()
    {
        RutineOn = false;
        StopAllCoroutines();
        
        Camera.main.transform.position = CameraPosition;
        canvas.gameObject.SetActive(false);
    }

    void Start()
    {
        RutineOn = false;
        canvas.gameObject.SetActive(false);
        CameraPosition = Camera.main.transform.position;
        BirdPosition = new Vector3(100f, -8f, 0f);
        DispenserPos = new Vector3(-215f, -20f, 0f);
        PoisonRect = Poison.GetComponent<RectTransform>();

    }

    IEnumerator BirdMove(Vector3 rightEnd, Vector3 leftEnd, float moveSpeed, float tiltAngle, float tiltSpeed, float stepDistance)
    {
        bool movingRight = true;  //ㅅㅂ 오른쪽왼쪽 반대로됨 걍살어 
        Vector3 targetPosition = rightEnd;  

        var BBird = Bird.GetComponent<RectTransform>();

        while (RutineOn)
        {
            // 새가 한쪽 끝에 도달할 때까지 이동
            while (Vector3.Distance(BBird.anchoredPosition, targetPosition) > 10f)
            {
                // 새를 stepDistance만큼 이동
                BBird.anchoredPosition = Vector3.MoveTowards(BBird.anchoredPosition, targetPosition, stepDistance * moveSpeed * Time.deltaTime);

                // 새가 기울어지도록 회전 (걷는 것처럼 보이게)
                float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltAngle;
                BBird.transform.rotation = Quaternion.Euler(0, movingRight ? 0 : 180, tilt);

                yield return null;  // 다음 프레임까지 대기
            }

            // 한쪽 끝에 도달했으므로 반대 방향으로 이동 설정
            movingRight = !movingRight;

            // 새의 Y축 회전 (오른쪽 → 왼쪽, 왼쪽 → 오른쪽)
            BBird.transform.rotation = Quaternion.Euler(0, movingRight ? 0 : 180, 0);

            // 목표 위치를 반대로 설정 (왼쪽 → 오른쪽, 오른쪽 → 왼쪽)
            targetPosition = movingRight ? rightEnd : leftEnd;
        }

        yield break;
    }

    

    public void PoisonDragStart()
    {
        PoisonRect = Poison.GetComponent<RectTransform>();
        
    }
    public void PoisonDrag()
    {
        Poison.transform.position = Input.mousePosition;
    }

    public void PoisonDragEnd()
    {
        if (Vector3.Distance(PoisonRect.anchoredPosition, DispenserPos) < 100f)
        {   
            Debug.Log("Poisoned !!!!");
            PoisonRect.anchoredPosition = DispenserPos;
            Poison.GetComponent<RawImage>().raycastTarget = false;
            StopAllCoroutines();
            StartCoroutine(BirdDying());
            IsSolved = true;
        }
        else
        {
            PoisonRect.anchoredPosition = PoisonPos;
        }
        
    }

    IEnumerator BirdDying()
    {
        RutineOn = false;
        float ttime = 0f;
        var BBird = Bird.GetComponent<RectTransform>();
        while (ttime < 3f)
        {
            ttime += Time.deltaTime;
            yield return null;
        }


        DeadBird.SetActive(true);
        Bird.SetActive(false);
        yield break;
    }

    public void NoteClicked()
    {
        InvenManager.ItemAdder("Bird_Note");
    }

}




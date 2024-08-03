using UnityEngine;
using System.Collections;

public class PlantPotMovement : MonoBehaviour
{
    public float moveDistance = 5f; // 이동할 거리
    public float moveDuration = 1f; // 이동에 걸리는 시간
    private bool isMoving = false; // 이동 중 여부
    private bool isMoved = false; // 이동 완료 여부
    private Vector3 originalPosition; // 원래 위치

    void Start()
    {
        originalPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0) && !isMoving)
        // {
        //     // 레이캐스트를 수행하여 클릭된 객체를 결정
        //     RaycastHit hit;
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         if (hit.transform == transform) // 클릭된 객체가 자신(PlantPot)인지 확인
        //         {
        //             if (isMoved)
        //             {
        //                 StartCoroutine(MovePlantPot(originalPosition)); // 원래 위치로 이동
        //             }
        //             else
        //             {
        //                 Vector3 targetPosition = originalPosition + new Vector3(-moveDistance, 0f, 0f);
        //                 StartCoroutine(MovePlantPot(targetPosition)); // 왼쪽으로 이동
        //             }
        //         }
        //     }
        // }
    }
    public void PlantPotClicked()
    {
        if (isMoved && !isMoving)
        {
            StartCoroutine(MovePlantPot(originalPosition)); // 원래 위치로 이동
        }  
        else if (!isMoving)
        {
            Vector3 targetPosition = originalPosition + new Vector3(-moveDistance, 0f, 0f);
            StartCoroutine(MovePlantPot(targetPosition)); // 왼쪽으로 이동
        }
    }
    IEnumerator MovePlantPot(Vector3 targetPosition)
    {
        isMoving = true; // 이동 중 플래그 설정
        Vector3 startPosition = transform.position; // 초기 위치
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration); // 서서히 이동
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        transform.position = targetPosition; // 정확히 목표 위치에 위치시킴
        isMoving = false; // 이동 종료 후 플래그 해제
        isMoved = !isMoved; // 이동 상태 반전
    }
}

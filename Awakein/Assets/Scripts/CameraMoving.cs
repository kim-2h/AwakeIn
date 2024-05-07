using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{   
    public Camera cam; // 확대할 카메라 이름
    public float zoomSpeed = 0.5f; // 확대 속도

    private Vector3 initialPosition; // 초기 카메라 위치
    private bool isFirstClick = true; // 첫 번째 클릭 여부

    void Start()
    {
        initialPosition = cam.transform.position; // 초기 위치 저장
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 만약 왼쪽 클릭이 발생하면
        {
            // 레이캐스트를 수행하여 클릭된 객체를 결정
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (isFirstClick)
                {
                    StartCoroutine(ZoomCoroutine(hit.point)); // 클릭된 객체 위치를 기준으로 확대 시작
                    isFirstClick = false;
                }
                else
                {
                    // 처음 상태로 되돌림
                    cam.transform.position = initialPosition;
                    isFirstClick = true;
                }
            }
        }
    }

    IEnumerator ZoomCoroutine(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = cam.transform.position;

        while (elapsedTime < 1.5f) // 2초 동안 확대
        {
            elapsedTime += Time.deltaTime * zoomSpeed; 
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 2f); // 초기 위치에서 목표 위치까지 Lerp

            yield return null; // 다음 프레임까지 대기
        }
    }
}

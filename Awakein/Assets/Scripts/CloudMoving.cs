using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoving : MonoBehaviour
{
    public float floatSpeed = 2.0f; // 위아래로 둥실거리는 속도
    public float floatAmplitude = 0.05f; // 둥실거리는 범위 (아주 작게)
    public float moveSpeed = 0.3f; // 1분에 화면의 맨 오른쪽에서 왼쪽으로 이동하는 속도 (초당 10/60 유닛)
    public float screenWidth = 10.0f; // 화면 너비

    private Vector3 initialPosition;

    void Start()
    {
        // 초기 위치를 저장
        initialPosition = transform.position;

        // Coroutine을 사용하여 동작 시작
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true) // 무한 반복 (Update와 같은 효과)
        {
            // 왼쪽으로 지속적으로 이동
            initialPosition.x -= moveSpeed * Time.deltaTime;

            // 위아래로 둥실거리는 효과 (초기 위치 기준)
            float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

            // 오브젝트가 화면 왼쪽 끝을 벗어나면 오른쪽으로 이동시킴
            if (initialPosition.x < -screenWidth / 2)
            {
                initialPosition.x = screenWidth / 2; // 화면 오른쪽 끝으로 재배치
            }

            // 새로운 위치로 업데이트
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

            // 다음 프레임까지 대기
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleAnim : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image image; // SpriteRenderer 대신 Image 사용
    public float spriteSpeed = 0.1f; // 스프라이트 애니메이션 속도
    public GameObject Airplane;

    // 비행기 이동 관련 변수
    public float moveAmplitude = 5f; // 이동 범위 (위아래 5f)
    public float moveSpeed = 2f; // 이동 속도 (사인 함수의 주기 제어)

    private int i = 0;
    private Vector3 initialPosition;

    void Start()
    {
        image = this.GetComponent<Image>();
        if(image == null)
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        if(sprites == null || sprites.Count == 0)
        {
            Debug.LogError("스프라이트 리스트가 비어 있습니다.");
            return;
        }

        // 비행기의 초기 위치 저장
        RectTransform rectTransform = Airplane.GetComponent<RectTransform>();
        if(rectTransform != null)
        {
            initialPosition = rectTransform.anchoredPosition;
        }
        else
        {
            initialPosition = Airplane.transform.position;
        }

        // 초기 위치 설정 (필요 시)
        Airplane.GetComponent<RectTransform>().anchoredPosition = initialPosition;

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float elapsedTime = 0f;

        while (true)
        {
            // 스프라이트 애니메이션 처리
            image.sprite = sprites[i];
            i = (i + 1) % sprites.Count; // 리스트의 크기에 맞게 인덱스 조정

            // 비행기 이동 처리
            float newYOffset = Mathf.Sin(elapsedTime * moveSpeed) * moveAmplitude;

            // RectTransform을 사용하는 경우
            RectTransform rectTransform = Airplane.GetComponent<RectTransform>();
            if(rectTransform != null)
            {
                rectTransform.anchoredPosition = initialPosition + new Vector3(0, newYOffset, 0);
            }
            else
            {
                // 일반 Transform을 사용하는 경우
                Airplane.transform.position = initialPosition + new Vector3(0, newYOffset, 0);
            }

            // 시간 업데이트
            elapsedTime += spriteSpeed;

            // 다음 프레임까지 대기
            yield return new WaitForSeconds(spriteSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleAnim : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image image; // SpriteRenderer 대신 Image 사용
    public float speed = 0.1f; // 기본 속도 설정
    int i = 0;

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
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            image.sprite = sprites[i];
            i = (i + 1) % sprites.Count; // 리스트의 크기에 맞게 인덱스 조정
            yield return new WaitForSeconds(speed);
        }
    }
}

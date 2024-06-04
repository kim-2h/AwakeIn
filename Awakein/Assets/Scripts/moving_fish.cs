using UnityEngine;

public class moving_fish : MonoBehaviour
{
    public float speed = 1.0f; // 움직이는 속도
    private float timer = 0.0f; // 경과 시간
    private bool isMoving = false; // 오른쪽으로 움직이는 중인지 여부

    public GameObject targetObject; 
     SpriteRenderer rend;
    private bool previousState;

    void Start()
    {
        previousState = targetObject.activeSelf;
        rend = GetComponent<SpriteRenderer>();
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (targetObject.activeSelf != previousState && !targetObject.activeSelf)
        {
            isMoving = true;
            timer = 0.0f; // 타이머 리셋
            gameObject.SetActive(true); 
        }

        // 오른쪽으로 움직이는 중이면
        if (isMoving)
        {
            // 3초 이하일 때는 오른쪽으로 이동
            if (timer < 3.0f)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            // 3초 이후부터 6초까지는 왼쪽으로 이동
            else if (timer < 6.0f)
            {
                rend.flipX = true;
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            // 6초가 지나면 멈추고 비활성화
            else
            {
                isMoving = false;
                gameObject.SetActive(false); // 6초 후 비활성화
            }

            timer += Time.deltaTime;
        }

        previousState = targetObject.activeSelf; 
    }
}

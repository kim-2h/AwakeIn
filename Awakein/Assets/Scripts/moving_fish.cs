using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moving_fish : MonoBehaviour
{
        public float timer = 3f; // 경과 시간
    private bool isMoving = false; // 오른쪽으로 움직이는 중인지 여부

    public GameObject targetObject; 
    private SpriteRenderer rend;
    private bool previousState;
    public GameObject Next;
    
   GameObject canvas;
    void Start()
    {
        previousState = targetObject.activeSelf;
        rend = GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("fishCanvas");
    }

    void Update()
    {
        if (targetObject.activeSelf != previousState && !targetObject.activeSelf)
        {
            isMoving = true;
            gameObject.SetActive(true); 
        }

        if (isMoving)
        {
            StartCoroutine(MoveFish());
            isMoving = false;
        }
    }

    IEnumerator MoveFish()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + 100, transform.position.y, transform.position.z);
        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / timer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        while( elapsedTime<6f&&elapsedTime>timer){
             transform.position = Vector3.Lerp(endPosition, startPosition, elapsedTime / (timer+3f));
             elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        gameObject.SetActive(false); 
        GameObject deadfish=Instantiate(Next,canvas.transform); 
       
    }
}

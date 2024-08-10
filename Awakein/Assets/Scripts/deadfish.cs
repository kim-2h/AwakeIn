using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadfish : MonoBehaviour
{

    private float timer = 2f;
    public GameObject targetObject, ClickBlocker; 
    private bool hasMoved = false;

    // Start is called before the first frame update
    void Start()
    {
        //previousState = targetObject.activeSelf;
        if (!hasMoved)
        {
            StartCoroutine(MoveFish());
            hasMoved = true;
        }
    }

    // Update is called once per frame
    // void Update()
    // {

    // } 
    IEnumerator MoveFish()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x , transform.position.y+50, transform.position.z);
        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / timer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       
        transform.position = endPosition;
        ClickBlocker.SetActive(false);

    }
}
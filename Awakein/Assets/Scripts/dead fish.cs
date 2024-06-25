using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadfish : MonoBehaviour
{
    public float speed = 1.0f; // Movement speed
    private float timer = 0.0f;
    public GameObject targetObject; 
    private bool previousState;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        previousState = targetObject.activeSelf;
       
        gameObject.SetActive(false); // Initially set this object to inactive
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the state of targetObject has changed
        if (targetObject.activeSelf != previousState)
        {
            
            isMoving = true;
            timer = 0.0f; // Reset the timer
            gameObject.SetActive(true); // Activate this object
        }

        // If isMoving is true, move the object
        if (isMoving)
        { 
            timer += Time.deltaTime;
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            // Stop moving after 3 seconds
            if (timer >= 3.0f)
            {
                isMoving = false;
               // gameObject.SetActive(false); // Deactivate this object after moving
            }
        }

    
    }
}

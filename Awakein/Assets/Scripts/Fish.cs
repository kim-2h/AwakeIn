using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Fish : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static Vector2 DefaultPos;
    public RectTransform Fishbowl;  // Change GameObject to RectTransform
    public float animationDuration = 2.0f;  // Duration of the animation
    public float animationDistance = 10.0f; // Distance to move up and down
    public Vector2 targetPos = new Vector2(115, 267); // Target position within the UI

    private Coroutine animationCoroutine;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = rectTransform.anchoredPosition;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out currentPos);
        rectTransform.anchoredPosition = currentPos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        RectTransform fishRectTransform = this.GetComponent<RectTransform>();

        if (IsOverlapping(fishRectTransform, Fishbowl))
        {
            this.rectTransform.Rotate(0,0,120);
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }
            //Debug.Log(gameObject.)
            animationCoroutine = StartCoroutine(AnimateFish(targetPos));
            //gameObject.SetActive(false);

        }
        else
        {
            rectTransform.anchoredPosition = DefaultPos;
        }
    }

    bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

        return rect1World.Overlaps(rect2World);
    }

    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    IEnumerator AnimateFish(Vector2 targetPosition)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float yOffset = Mathf.Sin((elapsedTime / animationDuration) * Mathf.PI * 2) * animationDistance;
            rectTransform.anchoredPosition = new Vector2(targetPosition.x, targetPosition.y + yOffset);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChange : MonoBehaviour
{
    public GameObject imageObject1; // 원래 이미지
    public GameObject imageObject2; // 바꿀 이미지

    // 버튼 클릭하면 바꿈
    public void SwitchImage()
    {
        bool isImage1Active = imageObject1.activeSelf;
        imageObject1.SetActive(!isImage1Active);
        imageObject2.SetActive(isImage1Active);
    }
}
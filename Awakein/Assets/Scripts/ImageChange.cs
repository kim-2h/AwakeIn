using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
   // public GameObject imageObject1; // 원래 이미지
   // public GameObject imageObject2; // 바꿀 이미지

    public  List<Texture> ImgScene1 = new List<Texture>();
    public  List<Texture> ImgScene2 = new List<Texture>();
    public  List<Texture> ImgScene3 = new List<Texture>();

    public void SwitchImage(GameObject TargetObj, string ImgName)
    {
        for (int i = 0; i < ImgScene1.Count; i++)
        {
            if (ImgName == ImgScene1[i].name && TargetObj.GetComponent<RawImage>().texture != null)
            {
                TargetObj.GetComponent<RawImage>().texture = ImgScene1[i];
            }
        }

        // bool isImage1Active = imageObject1.activeSelf;
        // imageObject1.SetActive(!isImage1Active);
        // imageObject2.SetActive(isImage1Active);
    }
    public void SwitchImage(GameObject TargetObj, int Idx)
    {
        TargetObj.GetComponent<RawImage>().texture = ImgScene1[Idx];
    }
}
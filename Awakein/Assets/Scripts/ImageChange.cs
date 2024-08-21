using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
   //public GameObject imageObject1; // 원래 이미지
   // public GameObject imageObject2; // 바꿀 이미지

    public  List<Texture> ImgScene1 = new List<Texture>();
    public  List<Sprite> SprScene1 = new List<Sprite>();
    public  List<Texture> ImgScene2 = new List<Texture>();
    public  List<Texture> ImgScene3 = new List<Texture>();
 
    public Image[] Alphalist;


    void Start()
    {
        foreach (var img in Alphalist)
        {
            img.alphaHitTestMinimumThreshold = 0.9f;
        }
    }
    public void SwitchImage(GameObject TargetObj, string ImgName)
    {
        for (int i = 0; i < ImgScene1.Count; i++)
        {
            var Raw = TargetObj.GetComponent<RawImage>();
            if (Raw != null && ImgName == ImgScene1[i].name && TargetObj.GetComponent<RawImage>().texture != null)
            {
                TargetObj.GetComponent<RawImage>().texture = ImgScene1[i];
                break;
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
    public void SwitchSprite(GameObject TargetObj, string ImgName)
    {
        for (int i = 0; i < SprScene1.Count; i++)
        {
            var Img = TargetObj.GetComponent<Image>();
            if (Img != null && ImgName == SprScene1[i].name && Img.sprite != null)
            {
                TargetObj.GetComponent<Image>().sprite = SprScene1[i];
                break;
            }
        }
    }
}
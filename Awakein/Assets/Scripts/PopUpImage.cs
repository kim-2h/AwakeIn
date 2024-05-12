using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Image myImage;
    public Sprite[] sprites;

    private int index = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            index = (index + 1) % sprites.Length;
            ChangeImage(index);
        }
    }

    void ChangeImage(int x)
    {
        if (x < 0 || x >= sprites.Length)
            return;

        myImage.sprite = sprites[x];
    }
}
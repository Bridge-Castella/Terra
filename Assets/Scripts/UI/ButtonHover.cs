using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour
{
    public Image[] hoverImg = new Image[2];
    public TextMeshProUGUI buttonText;

    void Start()
    {
        for(int i = 0; i < hoverImg.Length; i++)
            hoverImg[i].gameObject.SetActive(false);
        //buttonText.color = Color.grey;
    }

    public void MouseHoverOn()
    {
        AudioManager.instance.PlaySound("ui_03");
        for (int i = 0; i < hoverImg.Length; i++)
            hoverImg[i].gameObject.SetActive(true);
        //buttonText.color = Color.white;
        buttonText.fontStyle = FontStyles.Bold;
    }

    public void MouseHoverOff()
    {
        for (int i = 0; i < hoverImg.Length; i++)
            hoverImg[i].gameObject.SetActive(false);
        //buttonText.color = Color.grey;
        buttonText.fontStyle = FontStyles.Normal;
    }
}

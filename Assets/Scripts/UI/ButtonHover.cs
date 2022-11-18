using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour
{
    public Image[] hoverImg = new Image[2];
    public TextMeshProUGUI buttonText;

    public AK.Wwise.Event mouseHover;
    public AK.Wwise.Event mouseClick;
    public GameObject soundObject;

    void Start()
    {
        for(int i = 0; i < hoverImg.Length; i++)
            hoverImg[i].gameObject.SetActive(false);
        //buttonText.color = Color.grey;
    }

    public void MouseHoverOn()
    {
        //AudioManager.instance.PlaySound("ui_03");
        mouseHover.Post(soundObject);

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

    public void OnClickButton()
    {
        //AudioManaver.instance.PlaySound("ui_04");
        mouseClick.Post(soundObject);

        for (int i = 0; i < hoverImg.Length; i++)
            hoverImg[i].gameObject.SetActive(false);
        buttonText.fontStyle = FontStyles.Normal;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour
{
    public Image hoverImg;
    public TextMeshProUGUI buttonText;

    void Start()
    {
        hoverImg.gameObject.SetActive(false);
        //buttonText.color = Color.grey;
    }

    public void MouseHoverOn()
    {
        AudioManager.instance.PlaySound("ui_03");
        hoverImg.gameObject.SetActive(true);
        //buttonText.color = Color.white;
        buttonText.fontStyle = FontStyles.Bold;
    }

    public void MouseHoverOff()
    {
        hoverImg.gameObject.SetActive(false);
        //buttonText.color = Color.grey;
        buttonText.fontStyle = FontStyles.Normal;
    }
}

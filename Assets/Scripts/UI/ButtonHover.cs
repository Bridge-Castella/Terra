using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event mouseHover;
    [SerializeField] AK.Wwise.Event mouseClick;

    public void MouseHoverOn()
    {
        //AudioManager.instance.PlaySound("ui_03");                             // Outdated audio engine
        mouseHover.Post(gameObject);
    }

    public void OnClickButton()
    {
        //AudioManaver.instance.PlaySound("ui_04");                             // Outdated audio engine
        mouseClick.Post(gameObject);
    }
}

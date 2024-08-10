using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour
{
    public void MouseHoverOn()
    {
        UIAudio.Post(UIAudio.Instance.UI_in);
    }

    public void OnClickButton()
    {
        UIAudio.Post(UIAudio.Instance.UI_select);
    }
}

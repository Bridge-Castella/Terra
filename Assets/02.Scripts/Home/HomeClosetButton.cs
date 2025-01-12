using UnityEngine;
using UnityEngine.UI;

public class HomeClosetButton : HomeButtonBase
{
    [SerializeField] HomeCloset controller;

    public override void OnMouseEnter()
    {
        controller.OnHoverCloset();
    }

    public override void OnMouseExit()
    {
        controller.OnHoverEndCloset();
    }

    public override void OnMouseClick()
    {
        InGameAudio.Post(InGameAudio.Instance.House_OpenCloset);
        controller.OnClickCloset("옷");
    }
}

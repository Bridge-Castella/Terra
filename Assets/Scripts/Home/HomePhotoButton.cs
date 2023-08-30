using UnityEngine;
using UnityEngine.UI;

public class HomePhotoButton : HomeButtonBase
{
    static int hoverRefCount = 0;

    [SerializeField] HomePhoto controller;

    public override void OnMouseEnter()
    {
        hoverRefCount++;
        controller.OnEnterPhoto();
    }

    public override void OnMouseExit()
    {
        hoverRefCount--;
        if (hoverRefCount <= 0)
        {
            hoverRefCount = 0;
            controller.OnExitPhoto();
        }
    }

    public override void OnMouseClick()
    {
        hoverRefCount = 0;
        controller.OnExitPhoto();
        controller.OnClickPhoto();
    }
}

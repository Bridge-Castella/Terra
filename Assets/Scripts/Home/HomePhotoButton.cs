using UnityEngine;
using UnityEngine.UI;

public class HomePhotoButton : HomeButtonBase
{
    [System.Serializable]
    public enum PhotoType
    {
        Self = 0,
        Tree,
        Family,
        Town,
        Prophecy
    }

    [SerializeField] HomePhoto controller;
    [SerializeField] PhotoType photo;

    public override void OnMouseEnter()
    {
        controller.OnEnterPhoto(photo);
    }

    public override void OnMouseExit()
    {
        controller.OnExitPhoto(photo);
    }

    public override void OnMouseClick()
    {
        InGameAudio.Post(InGameAudio.Instance.House_SelectPicture);
        controller.OnExitPhoto(photo);
        controller.OnClickPhoto(photo);
    }
}

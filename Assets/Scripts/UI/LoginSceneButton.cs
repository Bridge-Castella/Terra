using UnityEngine;
using UnityEngine.EventSystems;

public class LoginSceneButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIAudio.Post(UIAudio.Instance.UI_pick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIAudio.Post(UIAudio.Instance.UI_in);
    }
}

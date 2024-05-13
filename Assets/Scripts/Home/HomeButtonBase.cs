using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class HomeButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool interactable = true;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_Hover);
        if (!interactable)
            return;

        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable)
            return;

        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable)
            return;

        OnMouseClick();
    }

    public abstract void OnMouseEnter();

    public abstract void OnMouseExit();

    public abstract void OnMouseClick();
}

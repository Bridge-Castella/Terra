using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomePhotoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    static int hoverRefCount = 0;

    [SerializeField] HomePhoto controller;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverRefCount++;
        controller.OnEnterPhoto();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverRefCount--;
        if (hoverRefCount <= 0)
        {
            hoverRefCount = 0;
            controller.OnExitPhoto();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        hoverRefCount = 0;
        controller.OnExitPhoto();
        controller.OnClickPhoto();
    }
}

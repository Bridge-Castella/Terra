using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonSFX : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private AK.Wwise.Event UI_hover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIAudio.Post(UI_hover);
    }
}

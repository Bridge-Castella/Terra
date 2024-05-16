using UnityEngine;
using UnityEngine.EventSystems;

public class PointerUp : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] AK.Wwise.Event wwiseEvent;

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        UIAudio.Post(wwiseEvent);
    }
}
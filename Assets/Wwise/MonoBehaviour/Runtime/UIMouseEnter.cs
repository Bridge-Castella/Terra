using UnityEngine.EventSystems;

public class UIMouseEnter : AkTriggerBase, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        triggerDelegate?.Invoke(null);
    }
}

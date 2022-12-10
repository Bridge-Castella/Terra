using UnityEngine.EventSystems;

public class UIMouseClick : AkTriggerBase, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        triggerDelegate?.Invoke(null);
    }
}

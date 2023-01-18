using UnityEngine.EventSystems;

public class UIMouseExit : AkTriggerBase, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        triggerDelegate?.Invoke(null);
    }
}

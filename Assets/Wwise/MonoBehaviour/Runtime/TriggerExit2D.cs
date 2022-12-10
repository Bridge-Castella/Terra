using UnityEngine;

public class TriggerExit2D : AkTriggerBase
{
    public GameObject triggerObject = null;

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (triggerDelegate != null && (triggerObject == null || triggerObject == collision.gameObject))
            triggerDelegate(collision.gameObject);
    }
}

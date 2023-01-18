using UnityEngine;

public class TriggerEnter2D : AkTriggerBase
{
    public GameObject triggerObject = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerDelegate != null && (triggerObject == null || triggerObject == collision.gameObject))
            triggerDelegate(collision.gameObject);
    }
}

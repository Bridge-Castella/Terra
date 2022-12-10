using UnityEngine;

public class CollisionEnter2D : AkTriggerBase
{
    public GameObject triggerObject = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggerDelegate != null && (triggerObject == null || triggerObject == collision.gameObject))
            triggerDelegate(collision.gameObject);
    }
}

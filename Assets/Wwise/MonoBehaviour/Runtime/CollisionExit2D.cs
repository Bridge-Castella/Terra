using UnityEngine;

public class CollisionExit2D : AkTriggerBase
{
    public GameObject triggerObject = null;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (triggerDelegate != null && (triggerObject == null || triggerObject == collision.gameObject))
            triggerDelegate(collision.gameObject);
    }
}

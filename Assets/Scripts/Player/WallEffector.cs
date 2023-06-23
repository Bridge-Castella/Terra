using System.Collections.Generic;
using UnityEngine;

public class WallEffector : MonoBehaviour
{
    [System.Serializable]
    public struct ActiveCondition
    {
        [Header("degree > contact normal vector")] public float degree;
        [Header("yVelocity < yVelocityThreshold")] public float yVelocityThreshold;
    }

    [System.Serializable]
    public struct TargetXVelocity
    {
        [Header("활성화시 벽에 접근했을때 해당 값으로 조정")] public bool changeXVelocity;
        [Header("xVelocity 조정시 고정 비율로 조정")] public bool fixedValue;
        [Header("xVelocity 조정시 해당 값으로 조정")] public float xVelocity;
    }

    [System.Serializable]
    public struct TargetFriction
    {
        [Header("활성화시 벽에 대한 friction 조정")] public bool changeFrictionValue;
        [Header("friction 조정시 해당 값으로 조정")] public float friction;
    }

    [System.Serializable]
    public struct Option
    {
        [Header("활성화 조건")] public ActiveCondition condition;
        [Header("xVelocity 조정")] public TargetXVelocity xVelocity;
        [Header("friction 조정")] public TargetFriction friction;
    }

    [SerializeField] Option option;
    [SerializeField] LayerMask layer;

    private bool isTouchingWall = false;

    // TODO: free values when it is not used
    private Dictionary<int, float> contactingObjects;
    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        contactingObjects = new Dictionary<int, float>();

        if (option.friction.changeFrictionValue)
        {
            Debug.LogError("Friction option is not compatible with grid collider. " +
                "Use platform collider 2d instead");
        }
    }

    public void CheckWall()
    {
        // if the collider is touching the wall
        if (isTouchingWall)
        {
            // check condition
            // this is only active when changeXVelocity is true and yVelocity is below yVelocityThreshold
            if (option.xVelocity.changeXVelocity &&
                rigid.velocity.y < option.condition.yVelocityThreshold)
            {
                // calculate final xVelocity considering the fixedValue
                // if fixedValue is true, desired velocity overwrites the xVelocity
                // if false, xVelocity will be xVelocity(ratio) * current xVelocity
                float xVal = option.xVelocity.fixedValue ?
                    option.xVelocity.xVelocity :
                    rigid.velocity.x * option.xVelocity.xVelocity;

                rigid.velocity = new Vector2(xVal, rigid.velocity.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // below values only concern about the friction
        if (!option.friction.changeFrictionValue)
            return;

        var coll = ChooseCollider(collision);

        // if the collision object does not have physics material, create default
        // then add its friction to the contactingObjects table
        if (coll.sharedMaterial == null)
        {
            coll.sharedMaterial = new PhysicsMaterial2D();
            contactingObjects.Add(coll.GetInstanceID(), coll.friction);
            return;
        }

        if (!contactingObjects.ContainsKey(coll.GetInstanceID()))
        {
            contactingObjects.Add(coll.GetInstanceID(), coll.friction);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & layer.value) > 0)
        {
            // get contact
            var contact = collision.GetContact(collision.contactCount - 1);

            // get contact normal, which represents the normal vector of contact point
            Vector2 contactNormal = contact.normal;

            Color debugColor = Color.green;

            // get angle: theta = Atan(y/x)
            // Abs is used to get angle at range 0 ~ 90
            float angle = Mathf.Atan(Mathf.Abs(contactNormal.y) / Mathf.Abs(contactNormal.x));

            // 90 - angle is used, since the angle is from normal vector
            float targetDegree = Mathf.Deg2Rad * (90 - option.condition.degree);

            // if angle is less than the desired degree, it is considered as touching the wall
            // it is only invoked when the condition is changed
            if (angle < targetDegree && !isTouchingWall)
            {
                // notify that the collider is touching wall
                isTouchingWall = true;

                // when the option says to change friction
                if (option.friction.changeFrictionValue)
                {
                    var coll = ChooseCollider(collision);

                    // change the friction in collider of collision object
                    coll.enabled = false;
                    coll.sharedMaterial.friction = option.friction.friction;
                    coll.enabled = true;
                }

                // red line is drawn, when it is touching the wall
                debugColor = Color.red;
            }
            else if (angle > targetDegree && isTouchingWall)
            {
                // notify that the collider is not touching wall
                isTouchingWall = false;

                if (option.friction.changeFrictionValue)
                {
                    var coll = ChooseCollider(collision);
                    int collId = coll.GetInstanceID();

                    if (contactingObjects.ContainsKey(collId))
                    {
                        // change the friction in collider of collision object
                        coll.enabled = false;
                        coll.sharedMaterial.friction = contactingObjects[collId];
                        coll.enabled = true;
                    }
                }
            }

            Debug.DrawRay(contact.point, contactNormal, debugColor);
        }
    }

    private Collider2D ChooseCollider(Collision2D collision)
    {
        return collision.collider.CompareTag("Player") ?
            collision.otherCollider : collision.collider;
    }
}

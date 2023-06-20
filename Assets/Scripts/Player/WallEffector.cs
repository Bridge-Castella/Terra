using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEffector : MonoBehaviour
{
    [SerializeField] float degree;
    [SerializeField] LayerMask layer;

    private bool isTouchingWall = false;

    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public void CheckWall()
    {
        if (isTouchingWall)
            rigid.velocity = new Vector2(.0f, rigid.velocity.y);
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

            // if angle is less than the desired degree, it is considered as touching the wall
            // 90 - angle is used, since the angle is from normal vector
            if (angle < (Mathf.Deg2Rad * (90 - degree)))
            {
                isTouchingWall = true;

                // red line is drawn, when it is touching the wall
                debugColor = Color.red;
            }
            else
            {
                isTouchingWall = false;
            }

            Debug.DrawRay(contact.point, contactNormal, debugColor);
        }
    }
}

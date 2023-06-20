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
            var contact = collision.GetContact(collision.contactCount - 1);
            Vector2 contactNormal = contact.normal;
            Color debugColor = Color.green;

            float angle = Mathf.Atan(Mathf.Abs(contactNormal.y) / Mathf.Abs(contactNormal.x));
            if (angle < (Mathf.Deg2Rad * (90 - degree)))
            {
                isTouchingWall = true;
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

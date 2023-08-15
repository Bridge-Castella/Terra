using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeProjectile : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] float torque;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] GameObject popEffect;

    private void Start()
    {
        float headed = transform.localScale.z > 0 ? -1f : 1f;
        rigid.AddForce(new Vector2(headed * force, 0f));
        rigid.AddTorque(torque);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(popEffect, gameObject.transform.position, Quaternion.identity);
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 3f);
        }
    }

    private void OnDestroy()
    {
        Instantiate(popEffect, gameObject.transform.position, Quaternion.identity);
    }
}

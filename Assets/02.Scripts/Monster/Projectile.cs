using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public float speed;
    public float force;
    private Transform player;

    public GameObject popEffect;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody2D = GetComponent<Rigidbody2D>();

        Vector3 direction = player.transform.position - transform.position;
        rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
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
        InGameAudio.Post(InGameAudio.Instance.ITEM_Destroy);
        Instantiate(popEffect, gameObject.transform.position, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterProjectile : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public float speed;
    public float force;

    private Transform player;

    public GameObject popEffect;
    public GameObject FlakeEffect;
    
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

    }
    public void Shoot(GameObject Target)
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        
        Vector3 direction = Target.transform.position - transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        else //if(collision.gameObject.CompareTag("Ground"))
        {
            //땅이면 화살 박히게 하려다 안박힘.
            rigidbody2D.velocity = new Vector2(0,0);
            rigidbody2D.angularVelocity = 0.0f;
            //gameObject.transform.position = gameObject.transform.position;
            FlakeEffect.active =false;
            Destroy(gameObject, 0.3f);
        }

    }

    private void OnDestroy()
    {
        
    }
}

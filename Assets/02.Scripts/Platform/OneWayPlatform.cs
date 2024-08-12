using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    bool fallThrough;
    GameObject player;

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            fallThrough = true;
        }
        else
        {
            fallThrough = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject; 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(fallThrough)
        {
            effector.rotationalOffset = 180f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        effector.rotationalOffset = 0f;
    }
}

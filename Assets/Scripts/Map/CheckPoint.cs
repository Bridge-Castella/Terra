using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private FallDetector fallDetector;
    private Animator animator;
    //마지막 지점이라는 것을 표시
    //public bool isEndPoint = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        fallDetector = FindObjectOfType<FallDetector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fallDetector.CheckPoint = gameObject.transform;
            //animator.SetTrigger("Move");
        }
        GetComponent<Collider2D>().enabled = false;
    }
}

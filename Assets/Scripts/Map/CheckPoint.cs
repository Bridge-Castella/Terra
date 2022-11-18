using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private FallDetector fallDetector;
    private Animator animator;
    private bool isChecked = false;
    //?????? ?????????? ???? ????
    public bool isEndPoint = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        fallDetector = FindObjectOfType<FallDetector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isChecked)
        {
            if(AudioManager.instance != null)
            {
                //if(isEndPoint)
                    //AudioManager.instance.PlaySound("stage_clear_01");
                //else
                    //AudioManager.instance.PlaySound("checkPoint_01");
            }
                
            if (collision.gameObject.tag == "Player")
            {
                fallDetector.CheckPoint = gameObject.transform;
                //animator.SetTrigger("Move");
            }
            isChecked = true;
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private FallDetector fallDetector;
    private Animator animator;
    private bool isChecked = false;
    //마지막 지점이라는 것을 표시
    public bool isEndPoint = false;

    [SerializeField] AK.Wwise.Event stageClear;
    [SerializeField] AK.Wwise.Event checkPoint;

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
                //if(isEndPoint)                                                // Outdated audio engine
                //AudioManager.instance.PlaySound("stage_clear_01");
                //else
                //AudioManager.instance.PlaySound("checkPoint_01");

                if (isEndPoint)
                    stageClear.Post(gameObject);
                else
                    checkPoint.Post(gameObject);
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

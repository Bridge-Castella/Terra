using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMonster : MovingPlatform
{
    Animator animator;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        //애니메이션 연산이 너무 많아서 프레임 드랍 현상. 카메라 뷰에 들어가면 애니메이션 시작하도록 함.
        Vector2 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            animator.speed = 1f;
            if (Vector2.Distance(wayPoints[currentWayPointIndex].transform.position, transform.position) < .1f)
            {
                currentWayPointIndex++;
                if (currentWayPointIndex >= wayPoints.Count)
                {
                    currentWayPointIndex = 0;
                }
                if(gameObject.GetComponent<SpriteRenderer>() != null)
                    gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
                StopAllCoroutines();
            }
            StartCoroutine(CoStopMovingWhenTurn());
        }
        else
            animator.speed = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        return;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        return;
    }

    IEnumerator CoStopMovingWhenTurn()
    {
        yield return new WaitForSeconds(2f);
        transform.position = Vector2.MoveTowards(transform.position,
                    wayPoints[currentWayPointIndex].transform.position, Time.deltaTime * speed);
        StartCoroutine(CoStopMovingWhenTurn());
    }
}

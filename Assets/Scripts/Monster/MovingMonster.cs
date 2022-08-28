using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMonster : MovingPlatform
{
    void Update()
    {
        if (Vector2.Distance(wayPoints[currentWayPointIndex].transform.position, transform.position) < .1f)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= wayPoints.Count)
            {
                currentWayPointIndex = 0;
            }
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            StopAllCoroutines();
        }
        StartCoroutine(CoStopMovingWhenTurn());
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] protected GameObject movingPlatformParent;
    [SerializeField] protected List<GameObject> wayPoints;
    [SerializeField] protected float speed = 2f;
    protected int currentWayPointIndex = 0;

    protected new Camera camera;

    public virtual void Start()
    {
        movingPlatformParent = gameObject.transform.parent.gameObject;

        int childCount = movingPlatformParent.transform.childCount;
        for(int i = 1; i<childCount;i++)
        {
            wayPoints.Add(movingPlatformParent.transform.GetChild(i).gameObject);
        }

        camera = Camera.main;
    }

    private void Update()
    {
        //애니메이션 연산이 너무 많아서 프레임 드랍 현상. 카메라 뷰에 들어가면 애니메이션 시작하도록 함.
        Vector2 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            if (Vector2.Distance(wayPoints[currentWayPointIndex].transform.position, transform.position) < .1f)
            {
                currentWayPointIndex++;
                if (currentWayPointIndex >= wayPoints.Count)
                {
                    currentWayPointIndex = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position,
                        wayPoints[currentWayPointIndex].transform.position, Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}

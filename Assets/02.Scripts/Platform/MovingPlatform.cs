using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject movingPlatformParent;
    [SerializeField] private List<GameObject> wayPoints;
    [SerializeField] private float speed = 2f;

    private int currentWayPointIndex = 1;
    
    private new Camera camera;

    bool isMoving = true;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;

    public void Start()
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
        if (isMoving)
        {
            if (Vector2.Distance(wayPoints[currentWayPointIndex].transform.position, transform.position) < .1f)
            {
                StartCoroutine(CoMoveIdle());
                currentWayPointIndex++;
                if (currentWayPointIndex >= wayPoints.Count)
                {
                    isMoving = false;
                    currentWayPointIndex = 0;
                }

                if (currentWayPointIndex == 1)
                    isMoving = false;
            }
            transform.position = Vector2.MoveTowards(transform.position,
                        wayPoints[currentWayPointIndex].transform.position, Time.deltaTime * speed);
        }
    }

    private IEnumerator CoMoveIdle()
    {
        isMoving = false;
        yield return new WaitForSeconds(idleDuration);
        isMoving = true;
    }

    // Fix: smoothing the jump animation, sticky wall
    //  - This fixes the slower move on moveing platform
    //  - second fix for non interactive other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject playerObj = null;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerObj = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("PlayerPlatformCollider"))
        {
            playerObj = collision.transform.parent.gameObject;
        }

        if (playerObj == null)
        {
            return;
        }

        playerObj.transform.SetParent(transform);
        playerObj.GetComponent<Rigidbody2D>().interpolation
            = RigidbodyInterpolation2D.Extrapolate;
    }

    // Fix: smoothing the jump animation, sticky wall
    //  - This fixes the slower move on moveing platform
    //  - second fix for non interactive other objects
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject playerObj = null;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObj = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("PlayerPlatformCollider"))
        {
            playerObj = collision.transform.parent.gameObject;
        }

        if (playerObj == null)
        {
            return;
        }

        playerObj.transform.SetParent(null);
        playerObj.GetComponent<Rigidbody2D>().interpolation
            = RigidbodyInterpolation2D.Interpolate;
    }

}
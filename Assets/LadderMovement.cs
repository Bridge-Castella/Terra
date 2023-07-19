using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LadderMovement : MonoBehaviour
{
    private Vector2[] points;
    
    public float Speed = 1.0f;

	void Start()
    {
        //Spline points 위치 초기화
        var edgeCollider = GetComponent<EdgeCollider2D>();
        var rawPoints = edgeCollider.points;

        points = new Vector2[rawPoints.Length];
		for (int i = 0; i < rawPoints.Length; i++)
		{
            points[i] = edgeCollider.transform.TransformPoint(rawPoints[i]);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerLadderCollider"))
        {
            collision.GetComponent<SplineMove>()
                .Activate(points, Speed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerLadderCollider"))
        {
            collision.GetComponent<SplineMove>().Deactivate();
        }
    }
}

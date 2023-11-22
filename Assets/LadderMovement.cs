using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LadderMovement : MonoBehaviour
{
    private Vector2[] points;
    private SplineMove moveCtrl;
    
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

    public void ActivateLadder()
    {
        moveCtrl?.Activate(points, Speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("PlayerLadderCollider"))
        {
            return;
        }

        moveCtrl = collider.GetComponent<SplineMove>();

        // TODO: activate ladder on full load
        ActivateLadder();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("PlayerLadderCollider"))
        {
            return;
        }

        if (collider.GetComponent<SplineMove>() != moveCtrl)
        {
            return;
        }

        moveCtrl?.Deactivate();
        moveCtrl = null;
    }
}

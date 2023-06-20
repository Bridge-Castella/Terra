using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LadderMovement : MonoBehaviour
{
    private Vector2[] points;
    private SplineMove spline;

    public float Speed = 1.0f;
    public float DistanceTrash = 0.5f;

	void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        spline = player.GetComponent<SplineMove>();

        //Spline points 위치 초기화
        var edgeCollider = GetComponent<EdgeCollider2D>();
        points = edgeCollider.points;
		for (int i = 0; i < points.Length; i++)
		{
            points[i] = edgeCollider.transform.TransformPoint(points[i]);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spline.Activate(points, Speed, DistanceTrash);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spline.Deactivate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [System.Serializable]
    public struct Range
    {
        public int begin;
        public int end;

        static public Range Init()
        {
			return new Range
            {
				begin = 0,
				end = -1
			};
        }
    }

    private Vector2[] points;
    private SplineMove spline;

    public float Speed = 1.0f;
    public float DistanceTrash = 0.5f;
    public Range scope = Range.Init();

	void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        spline = player.GetComponent<SplineMove>();

		//Spline points 위치 초기화
		points = gameObject.GetComponent<EdgeCollider2D>().points;
		Vector2 pos = transform.position;
		for (int i = 0; i < points.Length; i++)
		{
			points[i] += pos;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spline.Activate(points, Speed, DistanceTrash, scope);
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

using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private Vector2[] points;
    private SplineMove moveCtrl;
    private LadderOpacity opacity;
    private IEnumerator coActive;
    [SerializeField]
    private CanvasGroup btnCanvas;
    
    public float Speed = 1.0f;

	void Start()
    {
        opacity = GetComponentInChildren<LadderOpacity>();

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
        coActive = null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("PlayerLadderCollider"))
        {
            return;
        }

        btnCanvas.DOFade(1f, 0.3f);

        moveCtrl = collider.GetComponent<SplineMove>();
        coActive = WaitForLadderOpacity();
        StartCoroutine(coActive);
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

        if (coActive != null)
        {
            btnCanvas.DOFade(0f, 0.3f);
            StopCoroutine(coActive);
            return;
        }
       
        moveCtrl?.Deactivate();
        moveCtrl = null;
    }

    IEnumerator WaitForLadderOpacity()
    {
        yield return new WaitUntil(() => opacity.IsEnabled);
        ActivateLadder();
    }
}

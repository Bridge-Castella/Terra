using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public bool scrolling, parallax;

    public float backgroundSize;

    [SerializeField] float multiplier = 0.0f;
    [SerializeField] bool horizontalOnly = true;
    [SerializeField] bool verticalOnly = false;

    private Transform cameraTransform;

	private Vector3 startCameraPos;
	private Vector3 startPos;

    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

		if (!PlaceHolder.instance.contains(gameObject.name + "cameraPos"))
            startCameraPos = cameraTransform.position;
        else
            startCameraPos = PlaceHolder.instance.load<Vector3>(gameObject.name + "cameraPos");

        if (!PlaceHolder.instance.contains(gameObject.name + "startPos"))
            startPos = transform.position;
        else
            startPos = PlaceHolder.instance.load<Vector3>(gameObject.name + "startPos");


        layers = new Transform[transform.childCount];

        for(int i = 0;i<transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = layers.Length -1;
    }

	private void OnDestroy()
	{
		PlaceHolder.instance.store(gameObject.name + "cameraPos", startCameraPos);
        PlaceHolder.instance.store(gameObject.name + "startPos", startPos);
	}

	private void Update()
    {
        if(scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();
            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }
    }

    private void LateUpdate()
    {
        if (parallax)
        {
            var position = startPos;
            if (horizontalOnly)
                position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
            else if (verticalOnly)
                position.y += multiplier * (cameraTransform.position.y - startCameraPos.y);
            else
                position += multiplier * (cameraTransform.position - startCameraPos);
            transform.position = position;
        }
        
    }

    private void ScrollLeft()
    {
        int lastRight = rightIndex;

        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if(rightIndex < 0)
            rightIndex = layers.Length-1;
    }

    private void ScrollRight()
    {
        int lastRight = leftIndex;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }

    public void InitPosition()
    {
        if (PlaceHolder.instance.contains(gameObject.name + "cameraPos"))
            return;

		var rect = Camera.main.pixelRect;
		float ratio = rect.width / rect.height;
		float camera_width = Camera.main.orthographicSize * ratio * 2.0f;

		float direction = Camera.main.transform.position.x - transform.position.x;
		if (direction < 0.0f)
			camera_width *= -1.0f;

		var pos = transform.position;
		pos.x += camera_width * multiplier;
		transform.position = pos;
	}
}

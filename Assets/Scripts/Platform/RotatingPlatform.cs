using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateAngle = 30f;

    new Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        //애니메이션 연산이 너무 많아서 프레임 드랍 현상. 카메라 뷰에 들어가면 애니메이션 시작하도록 함.
        Vector2 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            transform.Rotate(0, 0, rotateAngle * Time.deltaTime * speed);
        }
    }
}

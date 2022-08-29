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
        //�ִϸ��̼� ������ �ʹ� ���Ƽ� ������ ��� ����. ī�޶� �信 ���� �ִϸ��̼� �����ϵ��� ��.
        Vector2 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            transform.Rotate(0, 0, rotateAngle * Time.deltaTime * speed);
        }
    }
}

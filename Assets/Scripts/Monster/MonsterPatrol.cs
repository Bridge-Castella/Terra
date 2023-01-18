using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform Edge1;
    [SerializeField] private Transform Edge2;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    new Camera camera;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        Vector2 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            if (movingLeft)
            {
                if (enemy.position.x >= Edge1.position.x)
                    MoveInDirection(-1);
                else
                {
                    DirectionChange();
                }
            }
            else
            {
                if (enemy.position.x <= Edge2.position.x)
                    MoveInDirection(1);
                else
                {
                    DirectionChange();
                }
            }
        }
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0f;
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * -_direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}

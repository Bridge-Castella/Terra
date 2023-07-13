using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClimbingMove : MonoBehaviour
{
    public enum State
    {
        Idle,
        Tracking,
        Grabbing,
        Climbing
    }

    [SerializeField] protected PlayerMove move;
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected Animator animator;
    [SerializeField] protected string animParam = "isClimbing";

    private static readonly Vector3 GroundUpVector = new Vector3(0, 0, 1f);
    private float originalGravity;

    protected State state { private set; get; }
    protected float speed = 1.0f;

    protected virtual void OnExit() { }
    protected virtual bool OnStart() { return true; }
    protected abstract State UpdateState(Vector2 input);
    protected abstract Vector2 UpdateDirection(Vector2 input);

    public void StartTracking() { state = State.Tracking; }
    public void ExitTracking() { state = State.Idle; }

    protected void Start()
    {
        state = State.Idle;
        originalGravity = move.gravityScale * move.fallGravityMultiflier;
    }

    private void FixedUpdate()
    {
        // 평상시 아무것도 안함
        if (state == State.Idle)
            return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 input = new Vector2(horizontalInput, verticalInput);

        // get new state
        State newState = UpdateState(input);

        if (newState != state)
            OnStateChange(newState);

        if (state != State.Climbing)
        {
            switch (state)
            {
                case State.Idle:
                case State.Tracking:
                    break;

                // dont move on grab
                case State.Grabbing:
                    FixMovement();
                    break;
            }

            return;
        }

        // get new direction
        Vector2 direction = UpdateDirection(input);

        // set gravity to 0
        rigid.gravityScale = 0;

        // 방향 벡터에 기반, 가속도 설정
        rigid.velocity = speed * move.maxSpeed * direction;
    }

    private void OnStateChange(State newState)
    {
        switch (newState)
        {
            // exit climbing
            case State.Tracking:
                _ExitClimb();
                break;

            case State.Grabbing:

                // if the terra wasnt climbing, then we should initialize values
                if (state == State.Tracking)
                    goto case State.Climbing;

                break;

            // start climbing
            case State.Climbing:
                if (!_StartClimb())
                    return;

                break;
        }

        // update state
        state = newState;
    }

    private void _ExitClimb()
    {
        // invoke callback
        OnExit();

        // exit climbing
        move.isClimbing = false;
        animator.SetBool(animParam, false);
        rigid.gravityScale = originalGravity;

        // rotate to normal degree
        rigid.gameObject.transform.Rotate(GroundUpVector, 0f);
    }

    private bool _StartClimb()
    {
        if (!OnStart())
            return false;

        // start climbing
        move.isClimbing = true;
        move.isJumping = false;
        animator.SetBool(animParam, true);

        return true;
    }

    private void FixMovement()
    {
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 0.0f;
    }
}

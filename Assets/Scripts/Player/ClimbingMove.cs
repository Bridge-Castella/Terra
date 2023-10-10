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

    [System.Serializable]
    public struct Settings
    {
        public string animParam;
        public float reEntryTime;
        public float speed;
    }

    [SerializeField] protected PlayerMove move;
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Settings settings = new Settings()
    {
        animParam = "isClimbing",
        reEntryTime = 0.5f,
        speed = 1.0f
    };

    private static readonly Vector3 GroundUpVector = new Vector3(0, 0, 1f);
    private float originalGravity;

    protected static bool waitForReEntry = true;
    private static int ownership = 0;
    private static int indexCounter = 0;
    private int index;

    protected State state { private set; get; }

    protected virtual void OnExit() { }
    protected virtual void OnStart() { }
    protected abstract State UpdateState(Vector2 input);
    protected abstract Vector2 UpdateDirection(Vector2 input);

    public void StartTracking() { state = State.Tracking; }
    public void ExitTracking() { state = State.Idle; }

    protected void Start()
    {
        index = ++indexCounter;
        state = State.Idle;
        originalGravity = move.gravityScale * move.fallGravityMultiflier;
    }

    private void FixedUpdate()
    {
        // if terra is unavailable to re-enter climbing we should not enter
        // only one climbing move can own the controll
        if (ownership != 0 && index != ownership)
            return;

        // do nothing on normal condition
        if (state == State.Idle)
        {
            // reset values to ensure that current climbing move has lost controll
            // fix: unable to retrive controll after exiting the ladder
            //  - now control is forced back to playermove when player has exited the ladder
            _ReleaseControll();
            return;
        }

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
        rigid.velocity = settings.speed * move.maxSpeed * direction;
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
                // if the terra was grabbing, then we should not initialize values
                if (state != State.Grabbing)
                    _StartClimb();
                break;
        }

        // update state
        state = newState;
    }

    private void _ExitClimb()
    {
        // invoke callback
        OnExit();

        // release current control
        // this will return control of terra back to playermove
        _ReleaseControll();

        // Wait for some time before re-entering the climbstate
        if (waitForReEntry)
        {
            // this makes no other climbing action to be entered
            ownership = -1;
            StartCoroutine(WaitForReEntry());
        }
    }

    private void _StartClimb()
    {
        // invoke callback
        OnStart();

        // start climbing
        move.isClimbing = true;
        move.isJumping = false;
        animator.SetBool(settings.animParam, true);
        waitForReEntry = true;

        // set mutex to true
        // this is to prevent stealing control from other climbing moves
        ownership = index;
    }

    private void FixMovement()
    {
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 0.0f;
    }

    private IEnumerator WaitForReEntry()
    {
        yield return new WaitForSeconds(settings.reEntryTime);
        ownership = 0;
    }

    private void _ReleaseControll()
    {
        // exit climbing
        move.isClimbing = false;
        animator.SetBool(settings.animParam, false);
        rigid.gravityScale = originalGravity;

        // rotate to normal degree
        rigid.gameObject.transform.Rotate(GroundUpVector, 0f);

        // reset mutex to release control of terra
        ownership = 0;
    }
}

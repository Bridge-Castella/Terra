using UnityEngine;
using UnityEngine.EventSystems;

public class AutoMoveTrigger : MonoBehaviour
{
    private int index;
    private bool shouldTrackUserInput; 
    private AutoMoveController controller;
    private Rigidbody2D playerRigid;

    private MoveDirection direction;
    private Vector3 difference;

    [System.Serializable]
    public enum AutoMoveOperation
    {
        None,
        FadeIn,
        FadeOut
    }

    // this specifies that it's start of the chain
    // start node can only start the auto move chain
    [Header("시작 구간임을 명시")] public bool StartNode;

    // start location of the auto move
    // this will affect the direction of the movement
    // player will be teleported to this location when bellow conditions are satisfied
    //  - Operation is FadeIn
    //  - Next trigger is not null
    [Header("시작위치")] public Transform StartPoint;

    // end location of the auto move
    // this will affect the direction of the movement
    // player will be teleported to this location when bellow conditions are satisfied
    //  - Operation is FadeIn
    //  - Next trigger is null
    [Header("목표지점")] public Transform Waypoint;

    // next trigger of the chain
    // next trigger will be executed at the end of the current trigger
    [Header("다음에 실행될 trigger")] public AutoMoveTrigger Next;

    // Operation to be performed
    [Header("수행할 옵션")]
    public AutoMoveOperation Operation;
    public float OperationDuration = -1f;

    public void Init(AutoMoveController controller, Rigidbody2D playerRigid, int index)
    {
        this.controller = controller;
        this.playerRigid = playerRigid;
        this.index = index;
    }

    private void Start()
    {
        difference = Waypoint.position - StartPoint.position;

        // get direction of the way point
        // the higher priority will remain, and the other axis will be ignored
        direction = Mathf.Abs(difference.x) > Mathf.Abs(difference.y) ?
            (difference.x > 0 ? MoveDirection.Right : MoveDirection.Left) :
            (difference.y > 0 ? MoveDirection.Up : MoveDirection.Down);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // fade should be triggered by the start node
        if (!StartNode || !collision.CompareTag("Player"))
            return;

        // when the user has none or incorrect input, we should not start chain
        // but this should be only checked for starting nodes
        if (!CheckInputDirection())
        {
            // but we still need to track it
            shouldTrackUserInput = true;
            return;
        }

        // just hoping for no bugs
        shouldTrackUserInput = false;

        // start the chain
        controller.StartAutoMoveChain(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // check for user input first
        // checking bool is faster than comparing string..?
        if (!shouldTrackUserInput || !collision.CompareTag("Player"))
            return;

        // start the chain when user have made correct input
        if (CheckInputDirection())
        {
            shouldTrackUserInput = false;
            controller.StartAutoMoveChain(this);
        }
    }

    private bool CheckInputDirection()
    {
        switch (direction)
        {
            // on horizontal move
            case MoveDirection.Left:
            case MoveDirection.Right:
                // if the input is correct, the value should be positive
                if (difference.x * Input.GetAxisRaw("Horizontal") <= 0f)
                    return false;
                break;

            // on vertical mvoe
            case MoveDirection.Up:
            case MoveDirection.Down:
                // if the input is correct, the value should be positive
                if (difference.x * Input.GetAxisRaw("Vertical") <= 0f)
                    return false;
                break;
        }
        return true;
    }

    private bool CheckVelocityDirection()
    {
        switch (direction)
        {
            // on horizontal move
            case MoveDirection.Left:
            case MoveDirection.Right:
                // if the input is correct, the value should be positive
                if (difference.x * playerRigid.velocity.x <= 0f)
                    return false;
                break;

            // on vertical mvoe
            case MoveDirection.Up:
            case MoveDirection.Down:
                // if the input is correct, the value should be positive
                if (difference.x * playerRigid.velocity.y <= 0f)
                    return false;
                break;
        }
        return true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (shouldTrackUserInput)
        {
            // since we don't need it anymore just deactivate it
            shouldTrackUserInput = false;
            return;
        }

        if (!collision.CompareTag("Player"))
            return;

        // we do not accept any slippery movement between the nodes
        if (!CheckVelocityDirection())
            return;

        // alert that player has reached end
        controller.MoveNext();
    }

    public override bool Equals(object other)
    {
        // override Equals method to check if it has the same index
        if (other is AutoMoveTrigger trigger)
            return index == trigger.index;

        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        // does not override has code
        return base.GetHashCode();
    }
}

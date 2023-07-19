using System.Collections.Generic;
using UnityEngine;

public class WallEffector : ClimbingMove
{
    [System.Serializable]
    public struct ActiveCondition
    {
        [Header("해당 값보다 벽면의 각도가 클 경우 적용"  )] public float degree;
        [Header("해당 값보다 yVelocity가 작을 경우 적용")] public float yVelocityThreshold;
    }

    [System.Serializable]
    public struct TargetXVelocity
    {
        [Header("활성화시 벽에 접근했을때 해당 값으로 조정")] public bool changeXVelocity;
        [Header("xVelocity 조정시 고정 값으로 조정"    )] public bool fixedValue;
        [Header("xVelocity 조정시 해당 값으로 조정"    )] public float xVelocity;
    }

    [System.Serializable]
    public struct Option
    {
        [Header("활성화 조건")] public ActiveCondition condition;
        [Header("xVelocity 조정")] public TargetXVelocity xVelocity;
    }

    private struct WallContactInfo
    {
        public Vector2 point;
        public Vector2 normal;
        public float normalAngle;
    }

    [SerializeField] Vector2 jumpEscapeScaleFactor = new Vector2(1.5f, 1.0f);
    [SerializeField] LayerMask layer;
    [SerializeField] Option option;

    private List<WallContactInfo> wallContactList;

    private WallContactInfo wallContact;
    private WallJump wallJump = new WallJump { state = WallJump.State.Null };
    private bool isClimbing;
    private float targetAngle;

    private PlayerAbilityTracker ability;

    private new void Start()
    {
        base.Start();

        wallContactList = new List<WallContactInfo>();
        ability = GetComponentInParent<PlayerAbilityTracker>();

        targetAngle = Mathf.Deg2Rad * (90 - option.condition.degree);
    }

    public Vector2 CheckWall()
    {
        // if the collider is touching the wall
        // this function is only affected on current frame
        // this is because OnCollisionStay2D is synchronized with the fixedupdate
        if (wallContactList.Count > 0)
        {
            // reset variables
            wallContactList.Clear();

            if (ability.canClimb)
            {
                // start tracking to climb wall
                StartTracking();
                isClimbing = true;
                return rigid.velocity;
            }

            // check condition
            // this is only active when changeXVelocity is true and yVelocity is below yVelocityThreshold
            if (option.xVelocity.changeXVelocity &&
                rigid.velocity.y < option.condition.yVelocityThreshold)
            {
                // calculate final xVelocity considering the fixedValue
                // if fixedValue is true, desired velocity overwrites the xVelocity
                // if false, xVelocity will be xVelocity(ratio) * current xVelocity
                float xVal = option.xVelocity.fixedValue ?
                    option.xVelocity.xVelocity :
                    rigid.velocity.x * option.xVelocity.xVelocity;

                return new Vector2(xVal, rigid.velocity.y);
            }
        }
        else
        {
            // when there is no other collision with wall, we should exit climbing
            if (isClimbing)
            {
                // exit tracking to exit climbing
                ExitTracking();
                isClimbing = false;
            }
        }

        if (wallJump.state != WallJump.State.Null)
            return CalculateWallJump();

        return rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collsion)
    {
        // wall jump should be deactivated no matter what the collision is
        wallJump.state = WallJump.State.Null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!CheckCollisionMask(collision))
            return;

        // test collision to check if terra is touching wall
        TestCollision(collision);
    }

    private void TestCollision(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            // get contact normal, which represents the normal vector of contact point
            var contactNormal = contact.normal;

            // get angle: theta = Atan(y/x)
            var normalAngle = Mathf.Atan(contactNormal.y / contactNormal.x);

            if (Mathf.Abs(normalAngle) < targetAngle)
            {
                // yellow line is drawn, when it is touching the wall
                Debug.DrawRay(contact.point, contactNormal, Color.yellow);

                wallContactList.Add(new WallContactInfo()
                {
                    point = contact.point,
                    normal = contact.normal,
                    normalAngle = normalAngle
                });
            }
            else
            {
                // blue line is drawn, when it is not touching the wall
                Debug.DrawRay(contact.point, contactNormal, Color.blue);
            }
        }
    }

    protected override State UpdateState(Vector2 input)
    {
        // 벽과의 충돌이 감지되지 않으면 탈출
        if (wallContactList.Count == 0)
            return State.Tracking;

        // save proper contact info
        wallContact = ChooseContact();

        // reset contact list for next frame
        wallContactList.Clear();
       
        //입력 없을 경우
        if (input.y == 0.0f)
        {
            // exit on jump
            if (move.isJumping)
            {
                // wall jump is only enabled when there is x axis input
                if (input.x != 0)
                {
                    // dont wait for re-entry when jumping from wall
                    waitForReEntry = false;
                    wallJump = new WallJump
                    {
                        state = WallJump.State.Begin,
                        StartDir = input
                    };
                }
                return State.Tracking;
            }
            // 바닥인데 아래로 갈려고 하는 경우 -> 탈출
            else if (move.IsGrounded())
                return State.Tracking;

            // 일단 벽에 붙기
            return State.Grabbing;
        }

        //입력이 존재할 때
        else
        {
            //reached below end, 아래 방향을 향할때
            if (move.IsGrounded() && input.y < 0f)
                return State.Tracking;

            return State.Climbing;
        }
    }

    protected override Vector2 UpdateDirection(Vector2 input)
    {
        int verticalInput = (int)input.y;
        float targetDegree = wallContact.normalAngle;
        Vector2 direction = Vector2.zero;

        // does not move on non input
        if (verticalInput == 0)
        {
            rigid.gravityScale = 0f;
            return Vector2.zero;
        }

        // go up
        if (verticalInput > 0)
        {
            targetDegree += Mathf.PI / 2;
            direction = new Vector2(Mathf.Cos(targetDegree),
                                    Mathf.Sin(targetDegree));
        }

        // go down
        else if (verticalInput < 0)
        {
            targetDegree -= Mathf.PI / 2;
            direction = new Vector2(Mathf.Cos(targetDegree),
                                    Mathf.Sin(targetDegree)) * 2;
        }

        bool wallFacingRight = wallContact.point.x - transform.position.x > 0;
        if (wallFacingRight != move.facingRight)
            move.Flip();

        // get direction of input
        return direction;
    }

    protected override void OnStart()
    {
        bool wallFacingRight = wallContact.point.x - transform.position.x > 0;
        if (wallFacingRight != move.facingRight)
            move.Flip();
    }

    private bool CheckCollisionMask(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & layer.value) > 0)
            return true;

        return false;
    }

    // TODO: choose contact by some condition
    private WallContactInfo ChooseContact()
    {
        // this returns last contact info only for temporary perpose
        return wallContactList[wallContactList.Count - 1];
    }


    struct WallJump
    {
        public enum State
        {
            Null,
            Begin,
            Doing
        }

        public State state;
        public Vector2 StartDir;
    }

    private Vector2 CalculateWallJump()
    {
        if (wallJump.StartDir.x * rigid.velocity.x < 0)
            wallJump.state = WallJump.State.Null;

        switch (wallJump.state)
        {
            case WallJump.State.Begin:
                wallJump.state = WallJump.State.Doing;
                return new Vector2(
                    rigid.velocity.x * jumpEscapeScaleFactor.x,
                    rigid.velocity.y * jumpEscapeScaleFactor.y);

            case WallJump.State.Doing:
                return new Vector2(
                    rigid.velocity.x * jumpEscapeScaleFactor.x,
                    rigid.velocity.y);

            case WallJump.State.Null:
                break;
        }

        return rigid.velocity;
    }
}

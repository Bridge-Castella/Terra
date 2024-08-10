using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EClimbState
{
    None,
    Ladder,
    Wall,
}

public class PlayerMove : MonoBehaviour
{
    [Header ("플랫폼 인식하는 레이어")]
    [SerializeField] private LayerMask platformLayerMask = default;
    [Header("NPC 인식하는 레이어")]
    [SerializeField] private LayerMask NPCLayerMask = default;
    [Header("중력 값")]
    [SerializeField] internal float gravityScale;
    [Header("떨어질때 중력에 곱하는 배수")]
    public float fallGravityMultiflier;

    [Header("플레이어 속도")]
    public float maxSpeed;
    [Header("플레이어 점프")]
    public float jumpPower;
    [Header("발자국 먼지 파티클")]
    public ParticleSystem dust;
    [Header("착지 먼지 파티클")]
    public GameObject landDustPrefab;

    [HideInInspector]public bool isHurting = false; //데미지 입은 경우

    Rigidbody2D rigid;
    WallEffector effector;
    [HideInInspector] public bool facingRight = true; //flip관련 bool 변수
    bool isKnockback = false; //튕겨나간 경우
    [HideInInspector] public bool isJumping = false; //점프하는 상태인 경우
    [HideInInspector] public bool isTalking = false;
    [HideInInspector] public bool isFalling = false;
    [HideInInspector] public bool isAutoMove = false; //Scene 변경관련 bool 변수

	//Scene 변경시 목표지점
	[HideInInspector] public Vector3 prefferedDirection;

    [HideInInspector] public EClimbState climbState = EClimbState.None;

	private CapsuleCollider2D capsuleCollider2D;
    private SpriteRenderer[] spriteRenderer = new SpriteRenderer[5];
    private Animator animator;
    private Color materialTintColor;

    //코요테 타임
    private float coyoteandJumpTime = 0.2f;
    private float coyoteandJumpTimeCounter;

    //능력, 아이템
    private PlayerAbilityTracker  abilities;
    private bool canDoubleJump = true;//더블 점프가 가능한지

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        for(int i = 0; i < 5; i++)
        {
            spriteRenderer[i] = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
        }
        
        animator = GetComponent<Animator>();        
        abilities = GetComponent<PlayerAbilityTracker>();
        effector = GetComponentInChildren<WallEffector>();

        if (GlobalContainer.contains("StartPos"))
            transform.position = GlobalContainer.load<Vector3>("StartPos");
    }

    void Update()
    {
        if (abilities.isFlying)
        {
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2(0, 0);
        }
        else if (isAutoMove)
        {
            //yesman: 바닥에 있는 동안은 점프 애니메이션을 출력하지 않음.
            animator.SetBool("isJumping", !IsGrounded() && climbState == EClimbState.None);
        }
        else
        {
            animator.SetFloat("yVelocity", rigid.velocity.y);

            if (IsGrounded())
            {
                coyoteandJumpTimeCounter = coyoteandJumpTime;
                //rigid.gravityScale = gravityScale;

                //yesman: 아이템 먹고 땅에 내려왔을때 jumppower 변경
                if (abilities.isSpringJump)
                {
                    jumpPower = abilities.springJumpPower;
                }
            }
            else
            {
                coyoteandJumpTimeCounter -= Time.deltaTime;
                rigid.gravityScale = gravityScale * fallGravityMultiflier;
            }

            //yesman: 바닥에 있는 동안은 점프 애니메이션을 출력하지 않음.
            animator.SetBool("isJumping", !IsGrounded() && climbState == EClimbState.None);

            //Coyote Time
            //Jump and Double Jump
            if ((Input.GetButtonDown("Jump") && 
                (IsGrounded() || climbState != EClimbState.None || 
                (canDoubleJump && abilities.canDoubleJump))) || 
                (coyoteandJumpTimeCounter > 0f && Input.GetButtonDown("Jump")))
            {
                Jump();
            }

            //yesman: 버튼 누른시간만큼 점프높이 높아짐
            if (Input.GetButtonUp("Jump"))
            {
                AddJump();
            }

            //Stop Speed
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }

            //e버튼 누르면
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isTalking)
                    NPCDialogue();
            }
        }
    }

    public void Jump()
    {
        PlayerAudio.Post(PlayerAudio.Instance.inGame_JUMP_01);

        // Climbing 중에도 double jump가 가능하게끔 수정
        if (IsGrounded())
        {
            if(climbState == EClimbState.None)
                canDoubleJump = true;
        }
        else
        {
            canDoubleJump = false;
            animator.SetTrigger("DougleJump");
        }

        if (abilities.isSpringJump)
        {
            Inventory.instance.Remove(Inventory.instance.SelectItem(400));
        }

        isJumping = true;
        rigid.velocity = Vector2.up * jumpPower;
        CreateDust();
    }

    private void AddJump()
    {
        //yesman: 스프링 아이템 먹고 점프 버튼 떼면 점프높이 원래대로
        if (abilities.isSpringJump && (jumpPower == abilities.springJumpPower))
        {
            abilities.isSpringJump = false;
            jumpPower /= 1.5f;
        }

        if (rigid.velocity.y > 0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
            coyoteandJumpTimeCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        //튕겨 나간 경우 방향키 입력x
        float moveHorizontalInput = Input.GetAxisRaw("Horizontal");
        float moveVerticalInput = Input.GetAxisRaw("Vertical");

        if (isKnockback || isFalling)
        {
            moveHorizontalInput = 0;
        }
        else if (isTalking)
        {
            //대화할때 움직이지 않도록
            rigid.velocity = new Vector2(0, 0);
            moveHorizontalInput = 0;
        }
        else if (climbState != EClimbState.None) return;
        else if (isAutoMove)
        {
            float multiplier;

            // 움직이고자 하는 방향이 zero라면 움직이지 않음
            if (prefferedDirection == Vector3.zero)
            {
                multiplier = 0f;
            }
            else
            {
                // normalize 비스무리한 값 산출
                // velocity가 horizontal.normalized + vertical.normalized로 적용되기에 아래와 같이 계산
                // 절대값이 더 큰 쪽을 10으로 만들고 나머지를 이에 비례하는 값으로 만듦
                multiplier = Mathf.Abs(prefferedDirection.x) > Mathf.Abs(prefferedDirection.y) ?
                    maxSpeed / Mathf.Abs(prefferedDirection.x) :
                    maxSpeed / Mathf.Abs(prefferedDirection.y);
            }

            rigid.velocity = IsGrounded() ? prefferedDirection * multiplier :
                // 점프중에는 진행방향 y velocity 무시
                // 기존 velocity의 y velocity를 그대로 사용
                new Vector3((prefferedDirection * multiplier).x, rigid.velocity.y, 0f);

            if (rigid.velocity.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (rigid.velocity.x < 0 && facingRight)
            {
                Flip();
            }
        }
        //날개 아이템 사용
        else if (abilities.isFlying)
        {
            rigid.velocity = new Vector2(moveHorizontalInput * maxSpeed, moveVerticalInput * maxSpeed);
            animator.SetBool("isJumping", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", abilities.isFlying);

            if (moveHorizontalInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveHorizontalInput < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            animator.SetBool("isFlying", abilities.isFlying);
            rigid.velocity = new Vector2(moveHorizontalInput * maxSpeed, rigid.velocity.y);
            rigid.velocity = effector.CheckWall();

            if (moveHorizontalInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveHorizontalInput < 0 && facingRight)
            {
                Flip();
            }
        }

        animator.SetFloat("xVelocity", Mathf.Abs(rigid.velocity.x));
    }

    public void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
        CreateDust();
    }

    public bool IsGrounded()
    {
        float extraHeightText = .35f;

        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.down, capsuleCollider2D.bounds.extents.y + extraHeightText, platformLayerMask);
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.down * (capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);

        return raycastHit.collider != null;
    }

    //착지할때 소리. 단, 점프하고 착지할때만 소리남
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 10 && isJumping)
        {
            PlayerAudio.Post(PlayerAudio.Instance.inGame_JUMP_Land);
            isJumping = false;
            CreateLandDust();  
        }
    }

    public void DamageFlash()
    {
        materialTintColor = new Color(1, 1, 1, 0.5f);
        for(int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = materialTintColor;
        }
        isHurting = true;        
    }

    public void DamageKnockBack(Vector3 targetPos, float knockBackPower = 30f)
    {
        //플레이어와 대상의 위치를 계산해서 반대쪽으로 튕기도록 방향 설정
        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        Vector2 knockBack = new Vector2(dir, 1) * knockBackPower;
        rigid.AddForce(knockBack, ForceMode2D.Impulse);
        DamageFlash();
        //부딪히면 나는 소리

        isKnockback = true;
        HeartManager.instance.GetDamage();

        StartCoroutine(CoEnableDamage(0.5f, 1.5f));
    }

    public IEnumerator CoEnableDamage(float waitTime1, float waitTime2)
    {
        if(isHurting)
        {
            yield return new WaitForSeconds(waitTime1);
            isKnockback = false;
            yield return new WaitForSeconds(waitTime2);
            materialTintColor = new Color(1, 1, 1, 1f);
            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                spriteRenderer[i].color = materialTintColor;
            }
            isHurting = false;
        }
    }

    void NPCDialogue()
    {
        rigid.velocity = Vector3.zero;
        //바라보는 방향 대로 raycast
        float x;
        if (facingRight)
             x = 1f;
        else
            x = -1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2D.bounds.center, new Vector2(x, 0), capsuleCollider2D.bounds.extents.y * 2f, NPCLayerMask);
        Color rayColor = Color.red;
        if(raycastHit.collider != null)
        {
            NpcAction npc = raycastHit.collider.GetComponent<NpcAction>();
            if (npc.IsDialogueEnd)
                return;
            isTalking = true;
            Debug.Log("NPC감지");
            GNBCanvas.instance.DialoguePanel.GetComponent<Dialogue>().npc = npc;
            
            if (!npc.ShowDialogueUIObject())
            {
                isTalking = false;
            }

        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, new Vector2(x, 0) * (capsuleCollider2D.bounds.extents.y) * 2f, rayColor);
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void CreateLandDust()
    {
        // TODO: temporary fix for null landDustPrefab
        if (landDustPrefab == null)
            return;

        GameObject obj = MonoBehaviour.Instantiate(landDustPrefab);
        obj.transform.position = gameObject.transform.position + new Vector3(0,-2.8f,0);
        ParticleSystem landDust = obj.GetComponent<ParticleSystem>();
        landDust.Play();
        Destroy(obj, 1f);
    }

    public void OnFootStep()
    {
        switch (MapManager.state.map)
        {
            case MapManager.MapIndex.Map1:
            case MapManager.MapIndex.Map3:
                PlayerAudio.Post(PlayerAudio.Instance.inGame_STEP_Grass);
                break;

            case MapManager.MapIndex.Map2:
                PlayerAudio.Post(PlayerAudio.Instance.inGame_STEP_Rock);
                break;

            default:
                PlayerAudio.Post(PlayerAudio.Instance.inGame_STEP_Grass);
                break;
        }
    }

    public void OnClimbStep()
	{
        switch (climbState)
        {
            case EClimbState.Ladder:
                PlayerAudio.Post(PlayerAudio.Instance.inGame_CLIMB_Ladder);
                break;
            
            case EClimbState.Wall:
                PlayerAudio.Post(PlayerAudio.Instance.inGame_CLIMB_Wall);
                break;
        }
	}
}

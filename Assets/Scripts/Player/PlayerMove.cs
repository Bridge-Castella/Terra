using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Tooltip ("플랫폼 인식하는 레이어")]
    [SerializeField] private LayerMask platformLayerMask = default;
    [Tooltip("NPC 인식하는 레이어")]
    [SerializeField] private LayerMask NPCLayerMask = default;
    [Tooltip("중력 값")]
    [SerializeField] private float gravityScale;
    [Tooltip("떨어질때 중력에 곱하는 배수")]
    public float fallGravityMultiflier;

    [Tooltip("플레이어 속도")]
    public float maxSpeed;
    [Tooltip("플레이어 점프")]
    public float jumpPower;
    [Tooltip("튕겨나가는 힘")]
    public float knockBackPower = 30f;

    [HideInInspector]public bool isHurting = false; //데미지 입은 경우

    Rigidbody2D rigid;
    [HideInInspector] public bool facingRight = true; //flip관련 bool 변수
    bool isKnockback = false; //튕겨나간 경우
    bool isJumping = false; //점프하는 상태인 경우
    [HideInInspector] public bool isTalking = false;
    [HideInInspector] public bool isFalling = false;

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
    }

    void Update()
    {
        if(abilities.isFlying)
        {
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2(0, 0);
        }
        else
        {
            animator.SetFloat("yVelocity", rigid.velocity.y);

            if (IsGrounded())
            {
                coyoteandJumpTimeCounter = coyoteandJumpTime;
                rigid.gravityScale = gravityScale;

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
            animator.SetBool("isJumping", !IsGrounded());

            //Coyote Time
            //Jump and Double Jump
            if ((Input.GetButtonDown("Jump") && (IsGrounded() || (canDoubleJump && abilities.canDoubleJump))) 
                || (coyoteandJumpTimeCounter > 0f && Input.GetButtonDown("Jump")))
            {
                if (AudioManager.instance != null)
                    AudioManager.instance.PlaySound("jump_01");

                if (IsGrounded())
                {
                    canDoubleJump = true;
                }
                else
                {
                    canDoubleJump = false;
                }

                isJumping = true;
                rigid.velocity = Vector2.up * jumpPower;
            }

            //yesman: 버튼 누른시간만큼 점프높이 높아짐
            if (Input.GetButtonUp("Jump"))
            {
                //yesman: 스프링 아이템 먹고 점프 버튼 떼면 점프높이 원래대로
                if(abilities.isSpringJump && (jumpPower == abilities.springJumpPower))
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

            //Stop Speed
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }

            //e버튼 누르면
            if (Input.GetButtonDown("TalktoNpc"))
            {
                if (!isTalking)
                    NPCDialogue();
            }
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
        else if(isTalking)
        {
            //대화할때 움직이지 않도록
            rigid.velocity = new Vector2(0, 0);
            moveHorizontalInput = 0;
        }
        //날개 아이템 사용
        else if(abilities.isFlying)
        {
            rigid.velocity = new Vector2(moveHorizontalInput * maxSpeed, moveVerticalInput * maxSpeed);

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
            rigid.velocity = new Vector2(moveHorizontalInput * maxSpeed, rigid.velocity.y);

            if (moveHorizontalInput != 0 && IsGrounded())
            {
                if (AudioManager.instance != null)
                    AudioManager.instance.PlayWalkSound("grass");
            }
            else
            {
                if (AudioManager.instance != null)
                    AudioManager.instance.StopWalkSound();
            }

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

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private bool IsGrounded()
    {
        float extraHeightText = .3f;

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
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySound("jump_02");
            isJumping = false;
        }
    }

    public void DamageFlash()
    {
        materialTintColor = new Color(1, 1, 1, 0.5f);
        for(int i = 0; i< spriteRenderer.Length; i++)
        {
            spriteRenderer[i].material.SetColor("_Color", materialTintColor);
        }
        isHurting = true;
    }

    public void DamageKnockBack(Vector3 targetPos, int damageAmount)
    {
        //플레이어와 대상의 위치를 계산해서 반대쪽으로 튕기도록 방향 설정
        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        Vector2 knockBack = new Vector2(dir, 1) * knockBackPower;
        rigid.AddForce(knockBack, ForceMode2D.Impulse);
        DamageFlash();
        //부딪히면 나는 소리
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySound("warn_01");

        isKnockback = true;
        HeartsHealthVisual.heartHealthSystemStatic.Damage(damageAmount);

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
                spriteRenderer[i].material.SetColor("_Color", materialTintColor);
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
            if (raycastHit.collider.GetComponent<NpcAction>().IsDialogueEnd || QuestManager.instance.isFailed)
                return;
            isTalking = true;
            Debug.Log("NPC감지");
            raycastHit.collider.GetComponent<NpcAction>().ShowDialogueUIObject();
            
        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, new Vector2(x, 0) * (capsuleCollider2D.bounds.extents.y) * 2f, rayColor);
    }
}

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
    bool facingRight = true; //flip관련 bool 변수
    bool isKnockback = false; //튕겨나간 경우
    bool isJumping = false; //점프하는 상태인 경우
    [HideInInspector] public bool isFalling = false;

    private CapsuleCollider2D capsuleCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color materialTintColor;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if(IsGrounded())
        {
            //땅에 있을때는 미끄러지지 않도록 1f로 변경
            rigid.sharedMaterial.friction = 1f;
            coyoteTimeCounter = coyoteTime;
            rigid.gravityScale = gravityScale;
        }
        else
        {
            //점프하면서 벽에 닿으면 붙지 않도록 0f로 설정
            rigid.sharedMaterial.friction = 0f;
            coyoteTimeCounter -= Time.deltaTime;
            rigid.gravityScale = gravityScale * fallGravityMultiflier;
        }

        //Jump
        if(coyoteTimeCounter > 0f && Input.GetButtonDown("Jump"))
        {
            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound("jump_01");
            isJumping = true;
            rigid.velocity = Vector2.up * jumpPower;
            //jumpTimeCounter = jumpTime;
        }

        if(Input.GetButtonUp("Jump") && rigid.velocity.y > 0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        if (Input.GetButtonDown("TalktoNpc"))
        {
            NPCDialogue();
        }
    }

    void FixedUpdate()
    {
        //Move By Key Control
        //float moveInput = Input.GetAxisRaw("Horizontal");

        /*rigid.AddForce(Vector2.right * moveInput, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);*/

        //튕겨 나간 경우 방향키 입력x
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (isKnockback || isFalling)
        {
            moveInput = 0;
        }
        else
        {
            rigid.velocity = new Vector2(moveInput * maxSpeed, rigid.velocity.y);

            if (moveInput != 0)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.Play("Idle");
            }

            if (moveInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && facingRight)
            {
                Flip();
            }
        }
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
        spriteRenderer.material.SetColor("_Color", materialTintColor);

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
            spriteRenderer.material.SetColor("_Color", materialTintColor);
            isHurting = false;
        }
    }

    void NPCDialogue()
    {
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
            Debug.Log("NPC감지");
            raycastHit.collider.GetComponent<NpcAction>().ShowDialogueUIObject();
        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, new Vector2(x, 0) * (capsuleCollider2D.bounds.extents.y) * 2f, rayColor);
    }
}

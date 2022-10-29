using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [SerializeField] private float fadePlatformTime;
    [SerializeField] private float showPlatformTime;

    private BoxCollider2D boxCol2D;
    private PolygonCollider2D polygonCol2D;
    //private SpriteRenderer spriteRend;
    private bool isFading;
    private Color platformAlpha;

    Animator animator;
    new Camera camera;

    private void Start()
    {
        //platformAlpha = gameObject.GetComponent<SpriteRenderer>().material.color;
        platformAlpha.a = 0;      

        boxCol2D = gameObject.GetComponent<BoxCollider2D>();
        polygonCol2D = gameObject.GetComponent<PolygonCollider2D>();
        //spriteRend = gameObject.GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    private void Update()
    {
        //�ִϸ��̼� ������ �ʹ� ���Ƽ� ������ ��� ����. ī�޶� �信 ���� �ִϸ��̼� �����ϵ��� ��.
        Vector2 viewPos = camera.WorldToViewportPoint(transform.position);
        if(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            animator.speed = 1f;
        }
        else
            animator.speed = 0f;

        if (isFading)
        {
            //�ִϸ��̼� sprite�� �� ��� ������� ȿ��
            /*gameObject.GetComponent<SpriteRenderer>().material.color = 
            Color.Lerp(gameObject.GetComponent<SpriteRenderer>().material.color, platformAlpha, fadingSpeed * Time.deltaTime);*/

            
        }

        //���� �� ��������� �ݶ��̴� false
        /*if(gameObject.GetComponent<SpriteRenderer>().material.color.a < 0.1f)
        {
            for (int i = 0; i < boxCol2D.Length; i++)
            boxCol2D[i].enabled = false;
            //spriteRend.enabled = false;
            isFading = false;       
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            animator.SetBool("isShowing", false);
            animator.SetBool("isQuaking", true);
            animator.Play("Quaking");
            StartCoroutine(CoFadingPlatform());
        }
    }

    /*public void ShowFadingPlatform()
    {
        StopAllCoroutines();
        animator.SetBool("isFalling", false);
        animator.SetBool("isQuaking", false);
        //gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < boxCol2D.Length; i++)
            boxCol2D[i].enabled = true;
    }*/

    IEnumerator CoFadingPlatform()
    {
        yield return new WaitForSeconds(fadePlatformTime);
        animator.SetBool("isFalling", true);
        animator.SetBool("isQuaking", false);
        boxCol2D.enabled = false;
        polygonCol2D.enabled = false;
        yield return new WaitForSeconds(showPlatformTime);
        ShowingPlatform();
    }

    private void ShowingPlatform()
    {
        animator.SetBool("isFalling", false);
        animator.SetBool("isQuaking", false);
        animator.SetBool("isShowing", true);
        boxCol2D.enabled = true;
        polygonCol2D.enabled = true;
    }
}

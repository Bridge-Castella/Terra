using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [SerializeField] private float fadingSpeed;
    [SerializeField] private float showPlatformTime;

    private BoxCollider2D[] boxCol2D;
    //private SpriteRenderer spriteRend;
    private bool isFading;
    private Color platformAlpha;

    Animator animator;

    private void Start()
    {
        //platformAlpha = gameObject.GetComponent<SpriteRenderer>().material.color;
        platformAlpha.a = 0;      

        boxCol2D = gameObject.GetComponents<BoxCollider2D>();
        //spriteRend = gameObject.GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(isFading)
        {
            /*gameObject.GetComponent<SpriteRenderer>().material.color = 
            Color.Lerp(gameObject.GetComponent<SpriteRenderer>().material.color, platformAlpha, fadingSpeed * Time.deltaTime);*/


            StartCoroutine(CoFadingPlatform());

        }
          
        /*if(gameObject.GetComponent<SpriteRenderer>().material.color.a < 0.1f)
        {
            
            //spriteRend.enabled = false;
            isFading = false;       
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            animator.SetBool("isQuaking", true);
            animator.Play("Quaking");
            isFading = true;
        }
    }

    public void ShowFadingPlatform()
    {
        animator.SetBool("isPlayerDead", true);
        //gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < boxCol2D.Length; i++)
            boxCol2D[i].enabled = true;
    }

    IEnumerator CoFadingPlatform()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("isFalling", true);
        for (int i = 0; i < boxCol2D.Length; i++)
            boxCol2D[i].enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenArea : MonoBehaviour
{
    private float disappearRate = 1f;

    private bool playerEntered;
    private SpriteRenderer wallSprite;

    float alphaValue = 1f;
    private void Start()
    {
        wallSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerEntered)
        {
            alphaValue -= Time.deltaTime * disappearRate;
            if(alphaValue <= 0f)
                alphaValue = 0f;

            wallSprite.color = new Color(wallSprite.color.r, wallSprite.color.g, wallSprite.color.b, alphaValue);
        }
        else
        {
            alphaValue += Time.deltaTime * disappearRate;
            if (alphaValue >= 1f)
                alphaValue = 1f;

            wallSprite.color = new Color(wallSprite.color.r, wallSprite.color.g, wallSprite.color.b, alphaValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.CompareTag("PlayerPlatformCollider") || collision.CompareTag("PlayerLadderCollider") || collision.CompareTag("Player"))
            playerEntered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPlatformCollider") || collision.CompareTag("PlayerLadderCollider") || collision.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }
}

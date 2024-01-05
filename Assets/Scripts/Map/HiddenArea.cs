using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenArea : MonoBehaviour
{
    private float disappearRate = 1f;
    private SpriteRenderer wallSprite;

    private void Start()
    {
        wallSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPlatformCollider") || collision.CompareTag("PlayerLadderCollider") || collision.CompareTag("Player"))
        {
            wallSprite.DOFade(0f, disappearRate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPlatformCollider") || collision.CompareTag("PlayerLadderCollider") || collision.CompareTag("Player"))
        {
            wallSprite.DOFade(1f, disappearRate);
        }
    }
}

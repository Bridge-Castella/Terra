using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingItem : Item
{
    [SerializeField] private float wingTime = 3f;
    [SerializeField] private float showTime = 2f;
    [SerializeField] GameObject itemSprite;

    public override void GetWingItem()
    {
        player.isFlying = true;
        itemSprite.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(CoUseWingItem());
    }

    IEnumerator CoUseWingItem()
    {
        yield return new WaitForSeconds(wingTime);
        player.isFlying = false;
        StartCoroutine(CoShowWingItem());
    }

    IEnumerator CoShowWingItem()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        itemSprite.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringItem : Item
{
    [SerializeField] private float springJumpMultiplier = 1.5f;
    [SerializeField] private float showTime = 2f;
    [SerializeField] GameObject itemSprite;

    public override void GetSpringItem()
    {
        itemSprite.SetActive(false);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        if(player.jumpPower != abilities.springJumpPower)
            abilities.springJumpPower = player.jumpPower * springJumpMultiplier;
        abilities.isSpringJump = true;
        StartCoroutine(CoShowSpringItem());
    }

    IEnumerator CoShowSpringItem()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        itemSprite.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItem : Item
{
    [SerializeField] GameObject itemSprite;

    //불 아이템은 1회성. 다시 안생김,
    //TODO: 인벤토리에 넣기
    public override void GetFireItem()
    {
        abilities.isHoldingFire = true;
        itemSprite.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}

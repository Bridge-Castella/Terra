using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItem : Item
{
    [SerializeField] private float showTime = 2f;
    [SerializeField] GameObject itemSprite;

    //�� �������� 1ȸ��. �ٽ� �Ȼ���,
    //TODO: �κ��丮�� �ֱ�
    public override void GetFireItem()
    {
        abilities.isHoldingFire = true;
        itemSprite.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}

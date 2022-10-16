using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /*public SkillItemObject skillItemObject;
    public int amount = 1;*/

    protected PlayerMove player;
    protected PlayerAbilityTracker abilities;

    protected enum ItemType
    { 
        Wing,  
        Spring,
        Fire,
    }

    [SerializeField] private ItemType itemType;

    //플레이어가 아이템에 부딪히면 아이템 얻음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO yeseul:아이템 얻을시 나는 소리: 종류마다 다르게 해야함
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySound("item_01");
        if (collision.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerMove>();
            abilities = collision.gameObject.GetComponent<PlayerAbilityTracker>();

            //날개, 스프링, 불 아이템 상속
            switch(itemType)
            { 
                case ItemType.Wing:
                    GetWingItem();
                    break;
                case ItemType.Spring:
                    GetSpringItem();
                    break;
                case ItemType.Fire:
                    GetFireItem();
                    break;
            }

            /*bool wasPickedUp = Inventory.instance.Add(skillItemObject, amount);

            if(wasPickedUp)
            {
                Destroy(gameObject);
            } */           
        }        
    }

    //게임을 종료하면 스킬아이템 amount가 0으로 변경됨
    /*private void OnApplicationQuit()
    {
        skillItemObject.amount = 0;
    }*/

    public virtual void GetWingItem()
    {
    }

    public virtual void GetFireItem()
    {
    }

    public virtual void GetSpringItem()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected PlayerMove player;
    protected PlayerAbilityTracker abilities;

    protected enum ItemType
    { 
        Wing,  
        Spring,
        Fire,
        Quest,
    }

    [SerializeField] private ItemType itemType;

    [SerializeField] AK.Wwise.Event itemCollect;

    public bool isStackable;
    public int amount;
    public int uid;
    public Sprite icon;
    public string itemName;
    public string desc;

    //?????????? ???????? ???????? ?????? ????
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO yeseul:?????? ?????? ???? ????: ???????? ?????? ??????
        itemCollect.Post(gameObject);
        if (collision.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerMove>();
            abilities = collision.gameObject.GetComponent<PlayerAbilityTracker>();

            //????, ??????, ?? ?????? ????
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
                case ItemType.Quest:
                    GetQuestItem();
                    break;
            }
        }        
    }

    public virtual void GetWingItem()
    {
    }

    public virtual void GetFireItem()
    {
    }

    public virtual void GetSpringItem()
    {
    }

    public virtual void GetQuestItem()
    {
    }
}

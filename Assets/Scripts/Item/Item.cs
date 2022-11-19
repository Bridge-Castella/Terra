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

    [SerializeField] AK.Wwise.Event itemCollect;

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
            }

            /*bool wasPickedUp = Inventory.instance.Add(skillItemObject, amount);

            if(wasPickedUp)
            {
                Destroy(gameObject);
            } */           
        }        
    }

    //?????? ???????? ?????????? amount?? 0???? ??????
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

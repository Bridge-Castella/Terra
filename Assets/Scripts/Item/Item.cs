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
    public GameObject pickupParticlePrefab;
    public GameObject pickupSubParticlePrefab;

    //플레이어가 아이템에 부딪히면 아이템 얻음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO yeseul:아이템 얻을시 나는 소리: 종류마다 다르게 해야함
        itemCollect.Post(gameObject);
        if (collision.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerMove>();
            abilities = collision.gameObject.GetComponent<PlayerAbilityTracker>();

            //날개, 스프링, 불 아이템 상속
            switch (itemType)
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
                    GetQuestItem(collision);
                    break;
            }
        }
        SpawnPickUpParticle();    
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

    public virtual void SpawnPickUpParticle()
    {
        if(pickupParticlePrefab)
        {
            GameObject obj = MonoBehaviour.Instantiate(pickupParticlePrefab);
            obj.transform.position = gameObject.transform.position;
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            particle.Play();
        }
        if(pickupSubParticlePrefab)
        {
            GameObject obj = MonoBehaviour.Instantiate(pickupSubParticlePrefab);
            obj.transform.position = gameObject.transform.position;
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            particle.Play();
        }
    }
    public virtual void GetQuestItem(Collider2D collision)
    {
    }
}

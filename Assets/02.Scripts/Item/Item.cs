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

    public bool isStackable;
    public int amount;
    public int uid;
    public string itemId;
    public Sprite icon;
    public string itemName;
    public string desc;
    public GameObject pickupParticlePrefab;
    public GameObject pickupSubParticlePrefab;

    //플레이어가 아이템에 부딪히면 아이템 얻음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerMove>();
            abilities = collision.gameObject.GetComponent<PlayerAbilityTracker>();

            InGameAudio.Post(InGameAudio.Instance.ITEM_Get_01);

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
            GameObject obj = Instantiate(pickupParticlePrefab);
            obj.transform.position = gameObject.transform.position;
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            particle.Play();
            Destroy(obj, 2f);
        }
        if(pickupSubParticlePrefab)
        {
            GameObject obj = Instantiate(pickupSubParticlePrefab);
            obj.transform.position = gameObject.transform.position;
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            particle.Play();
            Destroy(obj, 2f);
        }
    }
    public virtual void GetQuestItem(Collider2D collision)
    {
    }

    // 저장 파일에 기록할 정보
    [System.Serializable]
    public struct Save
    {
        public bool isStackable;
        public int amount;
        public int uid;
        public string itemId;
        public string itemName;
        public string desc;
    }

    public Save SaveData()
    {
        Save data = new Save();
        data.isStackable = isStackable;
        data.amount = amount;
        data.uid = uid;
        data.itemId = itemId;
        data.itemName = itemName;
        data.desc = desc;
        return data;
    }

    public void LoadData(Save data)
    {
        isStackable = data.isStackable;
        amount = data.amount;
        uid = data.uid;
        itemId = data.itemId;
        itemName = data.itemName;
        desc = data.desc;
        icon = TableData.instance.GetItemSprite(itemId);
    }
}

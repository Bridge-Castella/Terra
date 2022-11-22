using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;

    public int space = 4;

    public List<Item> items = new List<Item>();

    private bool isUsingItem = false;

    [SerializeField] AK.Wwise.Event itemLight;
    [SerializeField] AK.Wwise.Event itemFire;
    [SerializeField] AK.Wwise.Event itemWater;

    public bool Add(Item item, int amount)
    {
        if(item.isStackable)
        {
            if(items.Count > space)
            {
                Debug.Log("Not enough room.");
                return false;
            }

            bool itemAlreadyInInven = false;

            foreach(Item itemInven in items)
            {
                if(itemInven.uid == item.uid)
                {
                    itemInven.amount += amount;
                    itemAlreadyInInven = true;
                }
            }
            if(!itemAlreadyInInven)
            {
                items.Add(item);
            }

            if (OnItemChangedCallBack != null)
                OnItemChangedCallBack.Invoke();
            
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (OnItemChangedCallBack != null)
            OnItemChangedCallBack.Invoke();
    }

    private void Update()
    {
        //UseItem();
    }

    /*public void UseItem()
    {
        if(isUsingItem)
            return;

        foreach(SkillItemObject itemObject in itemObejcts)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(itemObject.skillType == SkillItemObject.SkillItemType.Light)
                {
                    //AudioManager.instance.PlaySound("item_02");               // Outdated audio engine
                    itemLight.Post(gameObject);
                    if (itemObject.amount > 0)
                    {
                        itemObject.amount--;
                        UseLightItem();
                        StartCoroutine(CoItemUserTimer());
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (itemObject.skillType == SkillItemObject.SkillItemType.Fire)
                {
                    //AudioManager.instance.PlaySound("item_03");               // Outdated audio engine
                    itemFire.Post(gameObject);                      
                    if (itemObject.amount > 0)
                        itemObject.amount--;
                }
                    
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (itemObject.skillType == SkillItemObject.SkillItemType.Water)
                {
                    //AudioManager.instance.PlaySound("item_04");
                    itemWater.Post(gameObject);                                 // Outdated audio engine
                    if (itemObject.amount > 0)
                        itemObject.amount--;
                }
            }
        }

        if(instance != null)
        {
            if (Inventory.instance.OnItemChangedCallBack != null)
                Inventory.instance.OnItemChangedCallBack.Invoke();
        }
        
    }*/

    public void UseLightItem()
    {
        PlayerMove player = ControlManager.instance.player.GetComponent<PlayerMove>();
        player.DamageFlash();
        StartCoroutine(player.CoEnableDamage(0f, 3f));
    }

    public IEnumerator CoItemUserTimer()
    {
        isUsingItem = true;
        yield return new WaitForSeconds(3f);
        isUsingItem = false;
    }

}

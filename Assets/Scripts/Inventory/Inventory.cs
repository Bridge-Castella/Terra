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
    public List<Item> skins = new List<Item>();

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
}

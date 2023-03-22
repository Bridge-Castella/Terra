using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    private void Start()
    {
        if (!GlobalContainer.contains("inventory"))
            return;

        Save data = GlobalContainer.load<Save>("inventory");
        LoadData(data);
    }

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

    [System.Serializable]
    public struct Save
    {
        public List<Item.Save> items;
        public List<Item.Save> skins;
    }

    public Save SaveData()
    {
        Save data = new Save();
        data.items = new List<Item.Save>();
        data.skins = new List<Item.Save>();

        // Items 저장
        foreach (Item item in items)
            data.items.Add(item.SaveData());

        // Skins 저장
        foreach (Item skin in skins)
            data.skins.Add(skin.SaveData());

        return data;
    }

    public void LoadData(Save data)
    {
        foreach (var item in data.items)
        {
            // Container 객체 생성
            GameObject itemObj = new GameObject(item.itemId);
            itemObj.transform.SetParent(transform);

            // Item 상태 Load
            Item item_copy = itemObj.AddComponent<Item>();
            item_copy.LoadData(item);

            // List에 추가
            items.Add(item_copy);
        }

        foreach (var skin in data.skins)
        {
            // Container 객체 생성
            GameObject itemObj = new GameObject(skin.itemId);
            itemObj.transform.SetParent(transform);

            // Skin 상태 Load
            Item skin_copy = itemObj.AddComponent<Item>();
            skin_copy.LoadData(skin);

            // List에 추가
            items.Add(skin_copy);
        }
    }
}

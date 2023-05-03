using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeItemGroup : MonoBehaviour
{
    HomeItem[] itemObjects;
    [SerializeField] HomeDetailPanel detailPanel;

    public void UpdateUI()
    {
        if (itemObjects == null)
            Init();

        var items = Inventory.instance.items;
        for (int i = 0; i < items.Count; i++)
        {
            var itemUI = itemObjects[i];
            var item = items[i];

            itemUI.UpdateUI(item.icon, item.amount, item.desc, i, detailPanel);
        }
    }

    private void Init()
    {
        itemObjects = new HomeItem[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            itemObjects[i] = transform.GetChild(i).GetComponent<HomeItem>();
    }
}

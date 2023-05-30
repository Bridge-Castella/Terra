using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeItemGroup : MonoBehaviour
{
    List<HomeItem> itemObjects;

    [SerializeField] GameObject homeItemPrefab;
    [SerializeField] HomeDetailPanel detailPanel;

    public void UpdateUI()
    {
        if (itemObjects == null)
            Init();

        var items = Inventory.instance.items;
        if (itemObjects.Count < items.Count)
        {
            for (int i = 0; i < 4; i++)
            {
                var item = GameObject.Instantiate(homeItemPrefab);
                item.transform.SetParent(this.transform);
                item.transform.localScale = new Vector3(1.0f, 1.0f);
                itemObjects.Add(item.GetComponent<HomeItem>());
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            var itemUI = itemObjects[i];
            var item = items[i];

            itemUI.UpdateUI(item.icon, item.amount, item.desc, i, detailPanel);
        }

        StartCoroutine(UpdateUILate());
    }

    private IEnumerator UpdateUILate()
    {
        // Wait single frame
        yield return null;

        // Match the rect size
        var parent_rect = transform.parent.GetComponent<RectTransform>();
        parent_rect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
    }

    private void Init()
    {
        itemObjects = new List<HomeItem>();
        for (int i = 0; i < transform.childCount; i++)
            itemObjects.Add(transform.GetChild(i).GetComponent<HomeItem>());
    }
}

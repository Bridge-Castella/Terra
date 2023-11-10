using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI amounText;
    public Image icon;
    public DetailPanel detailPanel;

    Item item;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickItemSlot);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        amounText.text = "";
    }

    public void OnClickItemSlot()
    {
        if(item != null)
        {
            detailPanel.icon.gameObject.SetActive(true);
            detailPanel.gameObject.SetActive(true);
            detailPanel.icon.sprite = icon.sprite;
            detailPanel.itemNameText.text = item.itemName;
            detailPanel.itemDescriptionText.text = item.desc;
        }
    }
}

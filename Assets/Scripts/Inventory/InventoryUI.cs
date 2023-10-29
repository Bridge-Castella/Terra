using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemSlotGroup;
    public Transform skinSlotGroup;
    [SerializeField] private DetailPanel detailPanel;
    [SerializeField] private Button exitButton;

    Inventory inventory;

    [SerializeField] public ItemSlot[] itemSlotArr;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        inventory = Inventory.instance;
        inventory.OnItemChangedCallBack += UpdateUI;
        itemSlotArr = itemSlotGroup.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < itemSlotArr.Length; i++)
        {
            itemSlotArr[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < inventory.space; i++)
        {
            itemSlotArr[i].gameObject.SetActive(true);
        }

        exitButton.onClick.AddListener(OnClickExitButton);

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < itemSlotArr.Length; i++)
        {
            if(i<inventory.items.Count)
            {
                itemSlotArr[i].AddItem(inventory.items[i]);
                itemSlotArr[i].amounText.text = inventory.items[i].amount.ToString();
                
            }
            //획득한 아이템 외의 다른 칸 클리어
            /*else
            {
                itemSlotArr[i].ClearSlot();
            }*/
        }
    }

    public void OnClickExitButton()
    {
        detailPanel.itemDescriptionText.text = "";
        detailPanel.itemNameText.text = "";
        detailPanel.icon.gameObject.SetActive(false);
    }
}

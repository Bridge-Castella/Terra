using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemSlotGroup;
    public Transform skinSlotGroup;
    [SerializeField] private DetailPanel detailPanel;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI slotTitle;

    Inventory inventory;

    [SerializeField] public ItemSlot[] itemSlotArr;

    private void Start()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnEnable()
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_Inventory_Open);

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
            else
            {
                itemSlotArr[i].ClearSlot();
            }
        }
    }

    public void OnClickExitButton()
    {
        if (!Home.IsHomeActive)
        {
            UIAudio.Post(UIAudio.Instance.inGame_UI_House_Close);
        }
        
        detailPanel.itemDescriptionText.text = "";
        detailPanel.itemNameText.text = "";
        detailPanel.icon.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetInventorySlotTitle(string title)
    {
        slotTitle.text = title;
    }
}

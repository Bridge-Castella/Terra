using UnityEngine;
using UnityEngine.UI;

public class HomeInventory : MonoBehaviour
{
    [SerializeField] Home homeController;
    [SerializeField] Image inventoryImage;
    [SerializeField] Sprite closedInventory;
    [SerializeField] Sprite openedInventory;

    [SerializeField] InventoryUI inventoryUI;

    private bool isOpened = false;

    public void OnHoverInventory()
    {
        if (!isOpened)
            inventoryImage.sprite = openedInventory;
    }

    public void OnHoverEndInventory()
    {
        if (!isOpened)
            inventoryImage.sprite = closedInventory;
    }

    public void OnClickInventory()
    {
        homeController.DisableButtons();
        homeController.ActivatePanelBackground(true);

        isOpened = true;
        inventoryImage.sprite = openedInventory;
        
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.skinSlotGroup.gameObject.SetActive(false);
        inventoryUI.itemSlotGroup.gameObject.SetActive(true);
        inventoryUI.UpdateUI();
    }

    public void OnExitInventory()
    {
        homeController.EnableButtons();
        homeController.ActivatePanelBackground(false);

        isOpened = false;
        inventoryImage.sprite = closedInventory;
        
        inventoryUI.gameObject.SetActive(false);
    }
}
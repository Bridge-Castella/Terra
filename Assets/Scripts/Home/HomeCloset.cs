using UnityEngine;
using UnityEngine.UI;

public class HomeCloset : MonoBehaviour
{
    [SerializeField] Home homeController;
    [SerializeField] Image closetImage;
    [SerializeField] Sprite closedCloset;
    [SerializeField] Sprite openedCloset;

    [SerializeField] InventoryUI inventoryUI;

    private bool isOpended = false;

    public void OnHoverCloset()
    {
        if (!isOpended)
            closetImage.sprite = openedCloset;
    }

    public void OnHoverEndCloset()
    {
        if (!isOpended)
            closetImage.sprite = closedCloset;
    }

    public void OnClickCloset()
    {
        homeController.DisableButtons();
        homeController.ActivatePanelBackground(true);
        isOpended = true;
        closetImage.sprite = openedCloset;
        
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.skinSlotGroup.gameObject.SetActive(true);
        inventoryUI.itemSlotGroup.gameObject.SetActive(false);
        inventoryUI.UpdateUI();
    }

    public void OnExitCloset()
    {
        homeController.EnableButtons();
        homeController.ActivatePanelBackground(false);
        isOpended = false;
        closetImage.sprite = closedCloset;
        
        inventoryUI.gameObject.SetActive(false);
        InGameAudio.Post(InGameAudio.Instance.House_CloseCloset);
    }
}

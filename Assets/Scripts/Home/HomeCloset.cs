using UnityEngine;
using UnityEngine.UI;

public class HomeCloset : MonoBehaviour
{
    [SerializeField] Home homeController;
    [SerializeField] Image closetImage;
    [SerializeField] Sprite closedCloset;
    [SerializeField] Sprite openedCloset;

    [SerializeField] GameObject Panel;

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
        Panel.SetActive(true);
    }

    public void OnExitCloset()
    {
        homeController.EnableButtons();
        homeController.ActivatePanelBackground(false);
        isOpended = false;
        closetImage.sprite = closedCloset;
        Panel.SetActive(false);
    }
}

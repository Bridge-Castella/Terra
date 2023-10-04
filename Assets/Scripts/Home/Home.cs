using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] HomeButtonBase[] buttons;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject PanelBackgroud;

    public void EnableButtons()
    {
        foreach (var button in buttons)
            button.interactable = true;

        exitButton.SetActive(true);
    }

    public void DisableButtons()
    {
        foreach (var button in buttons)
            button.interactable = false;

        exitButton.SetActive(false);
    }

    public void ActivatePanelBackground(bool active)
    {
        PanelBackgroud.SetActive(active);
    }
}

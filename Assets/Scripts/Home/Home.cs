using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] HomeButtonBase[] buttons;
    [SerializeField] GameObject PanelBackgroud;

    public void EnableButtons()
    {
        foreach (var button in buttons)
            button.interactable = true;
    }

    public void DisableButtons()
    {
        foreach (var button in buttons)
            button.interactable = false;
    }

    public void ActivatePanelBackground(bool active)
    {
        PanelBackgroud.SetActive(active);
    }
}

using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField] HomeButtonBase[] buttons;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject PanelBackgroud;

    private void OnEnable()
    {
        InGameAudio.Stop(MapStateChanger.CurrentMapBGM);
        InGameAudio.Post(InGameAudio.Instance.BGM_Terra_House_loop);
    }

    private void OnDisable()
    {
        InGameAudio.Stop(InGameAudio.Instance.BGM_Terra_House_loop);
        InGameAudio.Post(MapStateChanger.CurrentMapBGM);
    }

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

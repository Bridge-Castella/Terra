using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] HomeButtonBase[] buttons;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject PanelBackgroud;

    public static bool IsHomeActive { get; private set; } = false;

    private void Start()
    {
        exitButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_House_Open);
        IsHomeActive = true;

        InGameAudio.Stop(MapStateChanger.CurrentMapBGM);
        InGameAudio.Post(InGameAudio.Instance.BGM_Terra_House_loop);
    }

    private void OnDisable()
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_House_Close);
        IsHomeActive = false;

        InGameAudio.Stop(InGameAudio.Instance.BGM_Terra_House_loop);
        InGameAudio.Post(MapStateChanger.CurrentMapBGM);
    }

    public void EnableButtons()
    {
        foreach (var button in buttons)
            button.interactable = true;

        exitButton.gameObject.SetActive(true);
    }

    public void DisableButtons()
    {
        foreach (var button in buttons)
            button.interactable = false;

        exitButton.gameObject.SetActive(false);
    }

    public void ActivatePanelBackground(bool active)
    {
        PanelBackgroud.SetActive(active);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using DG.Tweening;

public class LoginManager : MonoBehaviour
{
    #region Singleton
    public static LoginManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
    #endregion

    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button ExitGameButton;
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button creditButton;

    [Header("--------------- Credit -----------------")]
    [SerializeField] private GameObject creditCanvas;
    [SerializeField] private CanvasGroup mainmenuContent;

    [Header("--------------- Prologue -----------------")]
    [SerializeField] private RawImage prologueRawImage;
    [SerializeField] private RenderTexture prologueTexture;
    [SerializeField] private VideoPlayer prologuePlayer;
    [SerializeField] private CanvasGroup skipBtn;

    [SerializeField] private GameObject optionObject;

    public static bool IsGameLoaded = false;

    public void Start()
    {
        //ÀúÀåÆÄÀÏÀÌ ÀÖ´Ù¸é continuebutton setactive true

        prologuePlayer.frame = 0;
        prologueTexture.Release();
        NewGameButton.onClick.AddListener(NewGame);
        ContinueButton.onClick.AddListener(ContinueGame);
        SettingButton.onClick.AddListener(OnClickSettingButton);
        ExitGameButton.onClick.AddListener(OnClickExitGame);
        creditButton.onClick.AddListener(() =>
        {
            creditCanvas.GetComponent<CreditCanvas>().FadeCredit(mainmenuContent);
        });
        prologueRawImage.GetComponent<Button>().onClick.AddListener(delegate
        {
            if (skipBtn.alpha == 0f)
            {
                skipBtn.DOFade(1f, 0.5f);
            }
        });
        skipBtn.GetComponent<Button>().onClick.AddListener(() => 
        {
            UIAudio.Post(UIAudio.Instance.UI_pick);
            EndReached(null);
        });
    }

    public void NewGame()
    {
        if (prologuePlayer.clip == null)
        {
            EndReached(null);
            return;
        }
        UIAudio.Post(UIAudio.Instance.UI_GameStart);

        prologueRawImage.gameObject.SetActive(true);
        prologueRawImage.DOColor(Color.white, 0.8f).OnComplete(() =>
        {
            LoginAudio.Stop(LoginAudio.Instance.LoginBGM);
            LoginAudio.Post(LoginAudio.Instance.PrologueBGM);

            prologuePlayer.Play();
            prologuePlayer.loopPointReached += EndReached;
        });
        
    }

    public void ContinueGame()
    {
        IsGameLoaded = true;

        SaveManager.SaveData? data_optional = SaveManager.LoadGame();
        if (data_optional == null)
            return;

        UIAudio.Post(UIAudio.Instance.UI_GameStart);
        LoginAudio.Stop(LoginAudio.Instance.LoginBGM);
        SaveManager.SaveData data = (SaveManager.SaveData)data_optional;
        MapManager.state.map = data.mapData.index;
        MapManager.state.cleared = data.mapData.cleared;
        MapManager.LoadMap(data.mapData.index);
    }

    public void OnClickExitGame()
    {
        UIAudio.Post(UIAudio.Instance.UI_pick);
        Application.Quit();
    }

    public void OnClickSettingButton()
    {
        UIAudio.Post(UIAudio.Instance.UI_pick);
        optionObject.SetActive(true);
    }

    void EndReached(VideoPlayer vp)
    {
        prologueRawImage.DOColor(Color.black, 0.8f).OnComplete(() =>
        {
            LoginAudio.Stop(LoginAudio.Instance.PrologueBGM);
            GlobalContainer.clear();
            MapManager.ResetData();
            MapManager.state.map = MapManager.MapIndex.Map1;
            MapManager.state.cleared = MapManager.MapIndex.Login;
            MapManager.LoadMap(MapManager.MapIndex.Map1);
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    #endregion

    public Button ContinueButton;
    public Button ExitGameButton;
    public Button SettingButton;
    public Button NewGameButton;

    public GameObject optionObject;


    public void Start()
    {
        //ÀúÀåÆÄÀÏÀÌ ÀÖ´Ù¸é continuebutton setactive true
        NewGameButton.onClick.AddListener(LoadScene);
        SettingButton.onClick.AddListener(OnClickSettingButton);
        ExitGameButton.onClick.AddListener(OnClickExitGame);
    }

    public void LoadScene()
    {
        MapManager.instance.mapState = MapManager.MapState.Forest;
        MapManager.instance.LoadMap((int)MapManager.MapState.Forest);
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OnClickSettingButton()
    {
        optionObject.SetActive(true);
    }
}

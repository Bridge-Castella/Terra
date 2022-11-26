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
        //AudioManager.instance.PlaySound("ui_04");
        MapManager.instance.mapState = MapManager.MapState.Forest;
        MapManager.instance.LoadMap();
    }

    public void OnClickExitGame()
    {
        //AudioManager.instance.PlaySound("ui_04");
        Application.Quit();
    }

    public void OnClickSettingButton()
    {
        //AudioManager.instance.PlaySound("ui_04");
        optionObject.SetActive(true);
    }
}

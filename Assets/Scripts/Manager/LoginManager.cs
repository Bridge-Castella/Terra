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
        NewGameButton.onClick.AddListener(NewGame);
        ContinueButton.onClick.AddListener(ContinueGame);
        SettingButton.onClick.AddListener(OnClickSettingButton);
        ExitGameButton.onClick.AddListener(OnClickExitGame);
    }

    public void NewGame()
    {
        MapManager.state.map = MapManager.MapIndex.Map1;
        MapManager.LoadMap(MapManager.MapIndex.Map1);
        GlobalContainer.clear();
    }

    public void ContinueGame()
    {
        SaveManager.SaveData? data_optional = SaveManager.LoadGame();
        if (data_optional == null)
            return;

        SaveManager.SaveData data = (SaveManager.SaveData)data_optional;
        MapManager.state.map = data.mapData.index;
        MapManager.LoadMap(data.mapData.index);
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

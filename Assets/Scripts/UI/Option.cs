using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Option : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Button audioButton;
    public Button saveButton;
    public Button quitButton;
    public Button initialButton;
    public Button cancelButton;
    public Button audioCancelButton;

    public GameObject mainMenuObject;
    public GameObject logoObject;

    public GameObject buttonGroup;
    public GameObject audioGroup;

    public GameObject popUpObject;
    public TextMeshProUGUI popUpText;
    public Button popUpYesButton;
    public Button popUpNoButton;

    public TextMeshProUGUI masterVolumeNum;
    public TextMeshProUGUI bgmVolumeNum;
    public TextMeshProUGUI sfxVolumeNum;

    [SerializeField] AK.Wwise.Event keyPause;
    [SerializeField] AK.Wwise.Event keyExit;

    private void Start()
    {
        //?????? ???? addlistener ??????????.
        audioButton.onClick.AddListener(OnClickAudioButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        initialButton.onClick.AddListener(InitailizeVolume);
        cancelButton.onClick.AddListener(OnClickCancelButton);

        popUpYesButton.onClick.AddListener(LoadLoginScene);
        popUpNoButton.onClick.AddListener(OnClickPopupNoButton);

        //???? ????
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        masterSlider.onValueChanged.AddListener(ChangeMasterVolumeNum);
        //??????
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        bgmSlider.onValueChanged.AddListener(ChangeBgmVolumeNum);
        //??????
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSfxVolumeNum);

        masterSlider.value = AudioManager.instance.masterVolumePercent;
        masterVolumeNum.text = Mathf.CeilToInt(masterSlider.value * 10).ToString();

        bgmSlider.value = AudioManager.instance.bgmVolumePercent;
        bgmVolumeNum.text = Mathf.CeilToInt(bgmSlider.value * 10).ToString();

        sfxSlider.value = AudioManager.instance.sfxVolumePercent;
        sfxVolumeNum.text = Mathf.CeilToInt(sfxSlider.value * 10).ToString();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //AudioManager.instance.PlaySound("ui_02");                         // Outdated audio engine
            keyPause.Post(gameObject);
            OnClickCancelButton();
        }
    }

    public void OnClickQuitButton()
    {
        //AudioManager.instance.PlaySound("ui_04");                             // Outdated audio engine
        keyExit.Post(gameObject);
        buttonGroup.SetActive(false);
        popUpObject.SetActive(true);
        popUpText.text = "?????? ?????????????????";
    }

    public void LoadLoginScene()
    {
        //AudioManager.instance.PlaySound("ui_04");                             // Outdated audio engine
        keyExit.Post(gameObject);
        Time.timeScale = 1;
        MapManager.instance.mapState = MapManager.MapState.Login;
        SceneManager.LoadScene("01.Login");

        //TODO ????: ?????? ???? ??????
        for(int i = 0; i< Inventory.instance.items.Count; i++)
        {
            Inventory.instance.items[i].amount = 0;
        }
    }

    public void OnClickPopupNoButton()
    {
        //AudioManager.instance.PlaySound("ui_04");                             // Outdated audio engine
        keyExit.Post(gameObject);
        popUpObject.SetActive(false);
        buttonGroup.SetActive(true);
    }

    public void OnClickCancelButton()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            if (MapManager.instance.mapState == MapManager.MapState.Login)
            {
                //AudioManager.instance.PlaySound("ui_02");                     // Outdated audio engine
                keyPause.Post(gameObject);
                mainMenuObject.SetActive(true);
                logoObject.SetActive(true);
                audioGroup.SetActive(false);
                buttonGroup.SetActive(true);
            }
            else
            {
                if(ControlManager.instance != null)
                {
                    ControlManager.instance.Resume();
                }    
            }
        }
    }



    #region Volume Control
    public void InitailizeVolume()
    {
        masterSlider.value = 0.5f;
        bgmSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
    }

    public void OnClickAudioButton()
    {
        //AudioManager.instance.PlaySound("ui_04");                             // Outdated audio engine
        keyExit.Post(gameObject);
        buttonGroup.SetActive(false);
        popUpObject.SetActive(false);
        audioGroup.SetActive(true);
    }
    public void SetBgmVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Bgm);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void ChangeMasterVolumeNum(float value)
    {
        masterVolumeNum.text =  Mathf.CeilToInt(value*10).ToString();
    }

    public void ChangeBgmVolumeNum(float value)
    {
        bgmVolumeNum.text = Mathf.CeilToInt(value * 10).ToString();
    }

    public void ChangeSfxVolumeNum(float value)
    {
        sfxVolumeNum.text = Mathf.CeilToInt(value * 10).ToString();
    }
    #endregion
}

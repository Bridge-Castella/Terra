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
    [SerializeField] private GameObject background;

    public GameObject buttonGroup;
    public GameObject audioGroup;

    public GameObject popUpObject;
    public TextMeshProUGUI popUpText;
    public Button popUpYesButton;
    public Button popUpNoButton;

    public GameObject popUpSave;
    public Button popUpSaveButton;

    public TextMeshProUGUI masterVolumeNum;
    public TextMeshProUGUI bgmVolumeNum;
    public TextMeshProUGUI sfxVolumeNum;

    private void Start()
    {
        //세이브 버튼 addlistener 추가해야함.
        audioButton.onClick.AddListener(OnClickAudioButton);
        saveButton.onClick.AddListener(OnClickSaveButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        initialButton.onClick.AddListener(InitailizeVolume);
        cancelButton.onClick.AddListener(() => 
        {
            if (LoginManager.instance == null)
            {
                UIAudio.Post(UIAudio.Instance.inGame_UI_House_Close);
            }
            else
            {
                UIAudio.Post(UIAudio.Instance.UI_optionOut);
            }

            if (MapManager.state.map != MapManager.MapIndex.Login)
            {
                
                GetComponentInParent<GNBCanvas>().Resume();
            }
            else
            {
                OnClickCancelButton();
            }
        });

        audioCancelButton.onClick.AddListener(() =>
        {
            if (LoginManager.instance == null)
            {
                UIAudio.Post(UIAudio.Instance.inGame_UI_House_Close);
            }
            else
            {
                UIAudio.Post(UIAudio.Instance.UI_optionOut);
            }
        });

        popUpYesButton.onClick.AddListener(LoadLoginScene);
        popUpNoButton.onClick.AddListener(OnClickPopupNoButton);

        // 저장 버튼, 인게임에서만 작동
        if (MapManager.state.map != MapManager.MapIndex.Login)
            popUpSaveButton.onClick.AddListener(OnClickPopUpSaveButton);

        //일반 볼륨
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        masterSlider.onValueChanged.AddListener(ChangeMasterVolumeNum);
        //배경음
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        bgmSlider.onValueChanged.AddListener(ChangeBgmVolumeNum);
        //효과음
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSfxVolumeNum);

        masterSlider.value = AudioManager.instance.masterVolumePercent;
        masterVolumeNum.text = Mathf.CeilToInt(masterSlider.value * 10).ToString();

        bgmSlider.value = AudioManager.instance.bgmVolumePercent;
        bgmVolumeNum.text = Mathf.CeilToInt(bgmSlider.value * 10).ToString();

        sfxSlider.value = AudioManager.instance.sfxVolumePercent;
        sfxVolumeNum.text = Mathf.CeilToInt(sfxSlider.value * 10).ToString();
    }

    public void OnMouseEnter()
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_Hover);
    }

    public void OnEnable()
    {
        UIAudio.Post(UIAudio.Instance.UI_in);
    }

    public void OnDisable()
    {
        // UIAudio.Post(UIAudio.Instance.UI_out);
    }

    public void OnClickSaveButton()
    {
        UIAudio.Post(UIAudio.Instance.UI_optionClick);
        SaveManager.SaveGame();
        buttonGroup.SetActive(false);
        popUpSave.SetActive(true);
    }

    public void OnClickQuitButton()
    {
        UIAudio.Post(UIAudio.Instance.UI_optionClick);
        buttonGroup.SetActive(false);
        popUpObject.SetActive(true);
        popUpText.text = "게임을 종료하시겠습니까?";
    }

    public void LoadLoginScene()
    {
        UIAudio.Post(UIAudio.Instance.UI_pick);
        Time.timeScale = 1;
        SaveManager.SaveGame();
        MapManager.state.map = MapManager.MapIndex.Login;
        SceneManager.LoadScene("01.Login");

        //TODO 임시: 아이템 개수 초기화
        for (int i = 0; i< Inventory.instance.items.Count; i++)
        {
            Inventory.instance.items[i].amount = 0;
        }

        LoginManager.IsGameLoaded = false;
    }

    public void OnClickAudioButton()
    {
        UIAudio.Post(UIAudio.Instance.UI_optionClick);
        buttonGroup.SetActive(false);
        popUpObject.SetActive(false);
        audioGroup.SetActive(true);
    }

    public void OnClickPopupNoButton()
    {
        UIAudio.Post(UIAudio.Instance.UI_optionClick);
        popUpObject.SetActive(false);
        buttonGroup.SetActive(true);
    }

    public void OnClickCancelButton()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            audioGroup.SetActive(false);
            buttonGroup.SetActive(true);
            popUpObject.SetActive(false);
            if (MapManager.state.map == MapManager.MapIndex.Login)
            {
                mainMenuObject.SetActive(true);
                logoObject.SetActive(true);
                audioGroup.SetActive(false);
                buttonGroup.SetActive(true);
            }
            else
            {
                /*if(ControlManager.instance != null)
                {
                    ControlManager.instance.Resume();
                } */   
            }
        }
    }

    public void OnClickPopUpSaveButton() 
    {
        UIAudio.Post(UIAudio.Instance.UI_optionClick);
        popUpSave.SetActive(false);
        buttonGroup.SetActive(true);
    }

    #region Volume Control
    public void InitailizeVolume()
    {
        UIAudio.Post(UIAudio.Instance.UI_optionClick);
        masterSlider.value = 0.5f;
        bgmSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
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
        // UIAudio.Post(UIAudio.Instance.UI_Soundbar);
    }

    public void ChangeBgmVolumeNum(float value)
    {
        bgmVolumeNum.text = Mathf.CeilToInt(value * 10).ToString();
        // UIAudio.Post(UIAudio.Instance.UI_Soundbar);
    }

    public void ChangeSfxVolumeNum(float value)
    {
        sfxVolumeNum.text = Mathf.CeilToInt(value * 10).ToString();
        // UIAudio.Post(UIAudio.Instance.UI_Soundbar);
    }
    #endregion
}

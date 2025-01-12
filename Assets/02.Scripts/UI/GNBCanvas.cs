using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GNBCanvas : MonoBehaviour
{
    public static GNBCanvas instance;

    [Header("----Menu----")]
    [SerializeField] private Button groupButton;
    [SerializeField] private Button questButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button homeButton;

    [Header("----Popup----")]
    [SerializeField] private CanvasGroup header;
    [SerializeField] private GameObject groupButtonContent;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private CreditCanvas creditCanvas;

    public GameObject OptionPanel
    {
        get { return optionPanel; }
        set { optionPanel = value; }
    }

    public CanvasGroup Header
    {
        get { return header; }
        set { header = value; }
    }

    public GameObject DialoguePanel
    {
        get { return dialoguePanel; }
        set { dialoguePanel = value; }
    }

    public CreditCanvas CreditCanvas
    {
        get { return creditCanvas; }
        set { creditCanvas = value; }
    }

    [Header("----Toast----")]
    [SerializeField] private Notice toastPopup;

    public bool gameIsPaused = false;

    #region Option
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Option();
    }

    public void Option()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //Destroy(optionObjectInstace);
        optionPanel.GetComponent<Option>().OnClickCancelButton();
        // content.SetActive(false);
        optionPanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        // content.SetActive(true);
        optionPanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    #endregion

    private void Start()
    {
        groupButton.onClick.AddListener(OnClickGroupButton);
        questButton.onClick.AddListener(OnClickQuestButton);
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
        homeButton.onClick.AddListener(OnClickHomeButton);
    }

    private void OnClickGroupButton()
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_Open);
        StopAllCoroutines();
        if (groupButtonContent.activeSelf)
        {
            groupButtonContent.SetActive(false);
            groupButtonContent.GetComponent<CanvasGroup>().DOFade(0f, 0.3f);
        }
        else
        {
            groupButtonContent.SetActive(true);
            groupButtonContent.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        }
    }

    private void OnClickQuestButton()
    {
        UIAudio.Post(UIAudio.Instance.inGame_UI_Quest_Open);
        //content.SetActive(true);
        questPanel.SetActive(true);
    }

    private void OnClickInventoryButton()
    {
        //content.SetActive(true);
        inventoryPanel.SetActive(true);
    }

    private void OnClickHomeButton()
    {
        //content.SetActive(true);
        homePanel.SetActive(true);
    }

    public void ShowToastPopup(string message)
    {
        toastPopup.Show(message);
    }

    /*IEnumerator CoWaitForAutoDisappear(float delay)
    {
        var images = groupButtonContent.GetComponentsInChildren<Image>();

        foreach (var image in images)
        {
            var color = image.color;
            color.a = 1f;
            image.color = color;
        }

        yield return new WaitForSeconds(delay);

        var timeElasped = 0f;
        var fadeTime = 1f;
        while (timeElasped < fadeTime)
        {
            timeElasped += Time.deltaTime;

            foreach (var image in images)
            {
                var color = image.color;
                color.a = (fadeTime - timeElasped) / fadeTime;
                image.color = color;
            }

            yield return null;
        }

        groupButtonContent.SetActive(false);
    }*/
}

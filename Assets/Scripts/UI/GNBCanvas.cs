using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class GNBCanvas : MonoBehaviour
{
    public static GNBCanvas instance;

    [Header("----Menu----")]
    [SerializeField] private Button groupButton;
    [SerializeField] private Button questButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button homeButton;

    [Header("----Popup----")]
    [SerializeField] private GameObject groupButtonContent;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private InventoryUI inventoryPanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject optionPanel;

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
        // TODO: resume audio

        //Destroy(optionObjectInstace);
        optionPanel.GetComponent<Option>().OnClickCancelButton();
        content.SetActive(false);
        optionPanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        // TODO: pause audio

        content.SetActive(true);
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
        StopAllCoroutines();
        StartCoroutine(CoWaitForAutoDisappear(3));
        groupButtonContent.SetActive(true);
    }

    private void OnClickQuestButton()
    {
        content.SetActive(true);
        questPanel.SetActive(true);
    }

    private void OnClickInventoryButton()
    {
        content.SetActive(true);
        inventoryPanel.gameObject.SetActive(true);
    }

    private void OnClickHomeButton()
    {
        content.SetActive(true);
        homePanel.SetActive(true);
    }

    public void ShowToastPopup(string message)
    {
        toastPopup.Show(message);
    }

    IEnumerator CoWaitForAutoDisappear(float delay)
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
    }
}

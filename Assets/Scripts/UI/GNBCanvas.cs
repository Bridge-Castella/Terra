using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class GNBCanvas : MonoBehaviour
{
    [SerializeField] private Button questButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private GameObject content;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject optionPanel;

    public bool gameIsPaused = false;

    [SerializeField] AK.Wwise.Event pause;
    [SerializeField] AK.Wwise.Event resume;

    #region Option

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
        if (resume != null)
            resume.Post(gameObject);

        //Destroy(optionObjectInstace);
        optionPanel.GetComponent<Option>().OnClickCancelButton();
        content.SetActive(false);
        optionPanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        if (pause != null)
            pause.Post(gameObject);

        content.SetActive(true);
        optionPanel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    #endregion

    private void Start()
    {
        questButton.onClick.AddListener(OnClickQuestButton);
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
        homeButton.onClick.AddListener(OnClickHomeButton);
    }

    private void OnClickQuestButton()
    {
        content.SetActive(true);
        questPanel.SetActive(true);
    }

    private void OnClickInventoryButton()
    {
        content.SetActive(true);
        inventoryPanel.SetActive(true);
    }

    private void OnClickHomeButton()
    {
        content.SetActive(true);
        homePanel.SetActive(true);
    }
}
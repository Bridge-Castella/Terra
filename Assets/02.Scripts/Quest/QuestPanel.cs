using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public Image portrait;
	public Image item;
	public TextMeshProUGUI title;
	public TextMeshProUGUI status;
    public TextMeshProUGUI description;

    public Button backButton;
    public Button stopButton;
    public Button mapButton;
    public Button exitButton;

    public GameObject stopPanel;
    public GameObject questPanelObject;
    public GameObject questListObject;
    private Quest quest;

    public void Init(Quest quest)
    {
        this.quest = quest;
        quest.submitCallback(OnUpdate);

        portrait.sprite = quest.portrait;
        item.sprite = quest.itemIcon;
        title.text = quest.data.npcId + ": " + quest.data.title;
        status.text = quest.data.status;
        description.text = quest.data.description;

        questPanelObject.SetActive(true);
        questListObject.SetActive(false);
    }

    private void OnUpdate()
    {
		status.text = quest.data.status;
    }

    private void Start()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnEnable()
	{
        UIAudio.Post(UIAudio.Instance.inGame_UI_Quest_Open);
        backButton.gameObject.SetActive(true);
        mapButton.gameObject.SetActive(true);
	}

	private void OnDisable()
	{
        UIAudio.Post(UIAudio.Instance.inGame_UI_Quest_Close);
        backButton.gameObject.SetActive(false);
        mapButton.gameObject.SetActive(false);
        questListObject.SetActive(true);
        questPanelObject.SetActive(false);
	}

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }


	public void stopQuest()
    {
        QuestManager.StopQuest(quest.data.npcId);
    }
}

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

    public GameObject stopPanel;
    public QuestSlotGroup slotGroup;
    private Quest quest;

    public void init(Quest quest)
    {
        this.quest = quest;
        quest.submitCallback(OnUpdate);

        portrait.sprite = quest.portrait;
        item.sprite = quest.itemIcon;
        title.text = quest.npcId + ": " + quest.title;
        status.text = quest.status;
        description.text = quest.description;
    }

    private void OnUpdate()
    {
		status.text = quest.status;
    }

	private void OnEnable()
	{
        backButton.gameObject.SetActive(true);
        mapButton.gameObject.SetActive(true);
	}

	private void OnDisable()
	{
        backButton.gameObject.SetActive(false);
        mapButton.gameObject.SetActive(false);
	}

	public void stopQuest()
    {
        QuestManager.instance.StopQuest(quest.npcId);
    }
}

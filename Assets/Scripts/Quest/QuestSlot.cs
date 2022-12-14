using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlot : MonoBehaviour
{
	//public Image portrait;
	public TextMeshProUGUI title;
	public TextMeshProUGUI status;
	public GameObject underLine;

	private QuestPanel questPanel;
	private GameObject questList;
	private Quest quest;

	private void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void init(Quest quest, QuestPanel panel, GameObject questList)
	{
		this.quest = quest;
		this.questPanel = panel;
		this.questList = questList;
		title.text = quest.npcId + " - " + quest.title;
		status.text = quest.status;
	}

	public void OnClick()
	{
		questPanel.init(quest);
		questPanel.gameObject.SetActive(true);
		questList.SetActive(false);
	}

	public void DisableUnderLine()
	{
		underLine.SetActive(false);
	}
}

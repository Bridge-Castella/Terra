using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class QuestSlot : MonoBehaviour
{
	public Image portrait;
	public Image Item;
	public TextMeshProUGUI title;
	public TextMeshProUGUI npcId;
	public TextMeshProUGUI status;

	private QuestPanel questPanel;
	private Quest quest;

	private void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void init(Quest quest, QuestPanel panel)
	{
		this.quest = quest;
		this.questPanel = panel;
		portrait.sprite = quest.portrait;
		Item.sprite = quest.itemIcon;
		title.text = quest.title;
		npcId.text = quest.npcId;
		status.text = quest.status;
	}

	public void OnClick()
	{
		questPanel.init(quest);
		questPanel.gameObject.SetActive(true);
		transform.parent.gameObject.SetActive(false);
	}
}

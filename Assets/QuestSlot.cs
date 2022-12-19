using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class QuestSlot : MonoBehaviour
{
    public Image portraitImage;
	public TextMeshProUGUI questTitle;
	public DetailPanel detailPanel;

	private Quest quest;

	public void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void init(string npdId, Quest quest, DetailPanel panel)
	{
		this.quest = quest;
		this.detailPanel = panel;
		portraitImage.sprite = TableData.instance.GetPortrait(quest.portraitId);
		questTitle.text = quest.title;
	}

	public void OnClick()
	{
		quest.updateState();
		detailPanel.gameObject.SetActive(true);
		detailPanel.icon.sprite = portraitImage.sprite;
		detailPanel.itemNameText.text = quest.title + "\n" + quest.statusStr;
		detailPanel.itemDescriptionText.text = quest.description;
	}
}

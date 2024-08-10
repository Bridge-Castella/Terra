using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSlotGroup : MonoBehaviour
{
    [SerializeField] GameObject questSlotPrefab;
	[SerializeField] GameObject emptyPrefab;
	[SerializeField] QuestPanel questPanel;
	[SerializeField] GameObject questList;

	private List<GameObject> questSlots = new List<GameObject>();

	private void Start()
	{
		QuestManager.submitOnQuestStart(OnQuestAdd);
	}

	private void OnEnable()
	{
		ClearQuests();
		UpdateQuests();
	}

	private void OnDisable()
	{
		ClearQuests();
	}

	private void OnQuestAdd(Quest quset)
	{
		ClearQuests();
		UpdateQuests();
	}

	private void UpdateQuests()
	{
		var quests = QuestManager.getActiveQuests();
		foreach (var quest in quests)
		{
			GameObject questSlot = Instantiate(questSlotPrefab, transform);
			questSlot.GetComponent<QuestSlot>().init(quest, questPanel, questList);
			questSlots.Add(questSlot);
		}

		if (quests.Length > 3)
			questSlots[quests.Length - 1].GetComponent<QuestSlot>().DisableUnderLine();

		for (int i = 0; i < 3 - quests.Length; i++)
		{
			GameObject emptySlot = Instantiate(emptyPrefab, transform);
			questSlots.Add(emptySlot);
		}
	}

	private void ClearQuests()
	{
		foreach (GameObject questSlot in questSlots)
		{
			Destroy(questSlot);
		}
		questSlots.Clear();
	}
}

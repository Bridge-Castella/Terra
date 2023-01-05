using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSlotGroup : MonoBehaviour
{
    [SerializeField] GameObject questSlotPrefab;
	[SerializeField] GameObject emptyPrefab;
	[SerializeField] QuestPanel questPanel;

	private List<GameObject> questSlots = new List<GameObject>();

	private void Start()
	{
		QuestManager.instance.onQuestStart += OnQuestAdd;
	}

	private void OnEnable()
	{
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
		List<Quest> quests = QuestManager.instance.getActiveQuests();
		foreach (Quest quest in quests)
		{
			GameObject questSlot = Instantiate(questSlotPrefab, transform);
			questSlot.GetComponent<QuestSlot>().init(quest, questPanel);
			questSlots.Add(questSlot);
		}

		if (quests.Count > 3)
			questSlots[quests.Count - 1].GetComponent<QuestSlot>().DisableUnderLine();

		for (int i = 0; i < 3 - quests.Count; i++)
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

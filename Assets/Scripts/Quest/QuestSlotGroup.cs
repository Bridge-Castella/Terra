using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSlotGroup : MonoBehaviour
{
    [SerializeField] GameObject questSlotPrefab;
	[SerializeField] QuestPanel questPanel;

	private List<GameObject> questSlots = new List<GameObject>();

	private void OnEnable()
	{
		List<Quest> quests = QuestManager.instance.getActiveQuests();
		foreach (Quest quest in quests)
		{
			GameObject questSlot = Instantiate(questSlotPrefab, transform);
			questSlot.GetComponent<QuestSlot>().init(quest, questPanel);
			questSlots.Add(questSlot);
		}
	}

	private void OnDisable()
	{
		foreach (GameObject questSlot in questSlots)
		{
			Destroy(questSlot);
		}
		questSlots.Clear();
	}
}

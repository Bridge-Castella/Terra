using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSlotGroup : MonoBehaviour
{
    [SerializeField] GameObject questSlotPrefab;
	[SerializeField] DetailPanel detailPanel;

	private List<GameObject> questSlots = new List<GameObject>();

	private void OnEnable()
	{
		List<(string, Quest)> quests = QuestManager.instance.getAllQuests();
		foreach ((string npcId, Quest quest) in quests)
		{
			GameObject questSlot = Instantiate(questSlotPrefab, transform);
			questSlot.GetComponent<QuestSlot>().init(npcId, quest, detailPanel);
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

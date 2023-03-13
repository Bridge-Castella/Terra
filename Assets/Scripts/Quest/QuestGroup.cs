using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroup : MonoBehaviour
{
	// List of quest objects
	[SerializeField] List<GameObject> questObject;

	// List of quests
	private List<Quest> questList;

	private void Start()
	{
		// Get npc id
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;

		// Add this QuestGroup to QuestManager
		QuestManager.instance.add(npcId, this);

		// Create a quest list from quest object list
		questList = new List<Quest>();
		foreach (GameObject questEle in questObject)
		{
			// Get quest
			Quest quest = questEle.GetComponent<Quest>();

			// initialize quest
			quest.init(npcId);
			questList.Add(quest);

			// If quest state already exits
			if (!QuestManager.instance.add(quest.questId, QuestState.Null))
			{
				// Get quest state
				QuestState state = (QuestState)QuestManager.instance.getState(quest.questId);

				// TODO: Get details on quest state
				if (state == QuestState.Doing)
					QuestManager.instance.StartQuest(quest, false);
				else if (state == QuestState.Succeeded)
					QuestManager.instance.StartQuest(quest, false);
			}
		}
	}

	// Destory current group
	private void OnDestroy()
	{
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.instance.delete(npcId);
	}

	// Find quest on quest list
	public Quest find(string questId)
	{
		foreach (Quest quest in questList)
		{
			if (quest.questId == questId)
				return quest;
		}
		return null;
	}

	public Quest at(int index)
	{
		if (questList.Count <= index)
			return null;
		return questList[index];
	}
}

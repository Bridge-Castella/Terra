using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroup : MonoBehaviour
{
	[Header("Quest Objects")]
	[SerializeField] List<GameObject> questObject;

	// List of quests
	private List<Quest> questList;

	private void Start()
	{
		// Get npc id
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;

		// Add this QuestGroup to QuestManager
		QuestManager.add(npcId, this);

        // Create a quest list from quest object list
        questList = new List<Quest>();
		foreach (GameObject questEle in questObject)
		{
			// Get quest
			Quest quest = questEle.GetComponent<Quest>();

			QuestState? state = QuestManager.getState(quest.questId);

			if (state != null)
			{
				QuestManager.StartQuest(quest, false);
				return;
			}

            // initialize quest
			quest.init(npcId);
			questList.Add(quest);

			// submit quest to manager
			QuestManager.add(quest.questId, QuestState.Null);
            
		}
	}

	// Destory current group
	private void OnDestroy()
	{
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.delete(npcId);
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

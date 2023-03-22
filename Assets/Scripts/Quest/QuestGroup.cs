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
		QuestManager.add(npcId, this);

		// Load substate from saved data
		var substate = GlobalContainer.contains("questData") ?
			GlobalContainer.load<Dictionary<string, int[]>>("questData") :
			null;

        // Create a quest list from quest object list
        questList = new List<Quest>();
		foreach (GameObject questEle in questObject)
		{
			// Get quest
			Quest quest = questEle.GetComponent<Quest>();

			// initialize quest
			quest.init(npcId);
			questList.Add(quest);

			// submit quest to manager
			QuestManager.add(quest.questId, QuestState.Null);

			// when substate is not in saved data
			if (substate == null)
				continue;
			if (!substate.ContainsKey(quest.questId))
				continue;

			// Load substate
			quest.loadData(substate[quest.questId]);

			// Start quest
			QuestManager.StartQuest(quest, false);
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

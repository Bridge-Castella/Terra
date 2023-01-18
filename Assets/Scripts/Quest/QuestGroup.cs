using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroup : MonoBehaviour
{
	[SerializeField] List<GameObject> questObject;
	private List<Quest> questList;

	private void Start()
	{
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.instance.add(npcId, this);

		questList = new List<Quest>();
		foreach (GameObject questEle in questObject)
		{
			Quest quest = questEle.GetComponent<Quest>();
			quest.init(npcId);
			questList.Add(quest);
			QuestManager.instance.add(quest.questId, QuestState.Null);
		}
	}

	private void OnDestroy()
	{
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.instance.delete(npcId);
	}

	public Quest find(string questId)
	{
		foreach (Quest quest in questList)
		{
			if (quest.questId == questId) return quest;
		}
		return null;
	}

	public Quest at(int index)
	{
		if (questList.Count <= index) return null;
		return questList[index];
	}
}

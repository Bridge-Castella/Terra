using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroup : MonoBehaviour
{
	public List<GameObject> questObject;
	private List<Quest> questList;

	public Quest current = null;

	private void Start()
	{
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.instance.add(npcId, this);
		questList = new List<Quest>();

		foreach (GameObject quest in questObject)
		{
			current = quest.GetComponent<Quest>();
		}
	}

	public void add(Quest quest)
	{
		questList.Add(quest);
		current = quest;
	}

	public void delete()
	{
		questList.Remove(current);
		current = null;
	}

	#nullable enable
	public Quest? find(string questId)
	{
		foreach (Quest quest in questList)
		{
			if (quest.questId == questId) return quest;
		}
		return null;
	}
	#nullable disable
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroup : MonoBehaviour
{
	[SerializeField] List<GameObject> questObject;
	[SerializeField] GameObject npc;

	private List<Quest> questList;
	private IEnumerator<Quest> questIter;

	private void Start()
	{
		string npcId = npc.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.instance.add(npcId, this);

		questList = new List<Quest>();
		foreach (GameObject questEle in questObject)
		{
			Quest quest = questEle.GetComponent<Quest>();
			questList.Add(quest);
		}

		questIter = questList.GetEnumerator();
		questIter.MoveNext();
	}

	public void moveToNextQuest()
	{
		questIter.MoveNext();
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

	public Quest? current => questIter.Current;
	#nullable disable
}

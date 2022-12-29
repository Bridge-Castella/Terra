using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        questGroup = new Dictionary<string, QuestGroup>();
        questState = new Dictionary<string, QuestState>();
        questIndex = new Dictionary<string, int>();
    }
    #endregion

    private Dictionary<string, QuestGroup> questGroup;
    private Dictionary<string, QuestState> questState;
    private Dictionary<string, int> questIndex;


    public void add(string npcId, QuestGroup questGroup)
    {
        this.questGroup.Add(npcId, questGroup);
        if (!questIndex.ContainsKey(npcId))
        {
            questIndex.Add(npcId, 0);
        }
    }

    public void delete(string npcId)
    {
        questGroup.Remove(npcId);
    }

    public void add(string questId, QuestState state)
    {
        if (questState.ContainsKey(questId)) return;
        questState.Add(questId, state);
    }


    public void StartQuest(string npcId, string questId)
    {
        Quest quest = questGroup[npcId].find(questId);
		if (quest == null)
        {
            Debug.Log("QuestManager : Quest not found");
            return;
        }

		QuestState state = questState[questId];
		if (state != QuestState.Null) return;

		questState[questId] = QuestState.Doing;
		quest.gameObject.SetActive(true);
		quest.startQuest();
        quest.gameObject.transform.parent = transform;
    }

    public void StopQuest(string npcId)
    {
		Quest quest = getActiveQuestByNpc(npcId);
		if (quest == null)
		{
			Debug.Log("QuestManager : Quest not found");
			return;
		}

        questState[quest.questId] = QuestState.Failed;
        Destroy(quest.gameObject);
        questIndex[npcId] += 1;
	}

    public void SucceedQuest(string npcId)
    {
		Quest quest = getActiveQuestByNpc(npcId);
		if (quest == null)
		{
			Debug.Log("QuestManager : Quest not found");
			return;
		}

		questState[quest.questId] = QuestState.Completed;
        Destroy(quest.gameObject);
		questIndex[npcId] += 1;
	}


    public List<Quest> getActiveQuests()
    {
        List<Quest> questList = new List<Quest>();
        foreach (Quest quest in GetComponentsInChildren<Quest>())
        {
            questList.Add(quest);
        }
        return questList;
    }

    public Quest getActiveQuest(string questId)
    {
		foreach (Quest quest in GetComponentsInChildren<Quest>())
		{
            if (quest.questId == questId) return quest;
		}
        return null;
	}

    public Quest getActiveQuestByNpc(string npcId)
    {
		foreach (Quest quest in GetComponentsInChildren<Quest>())
		{
			if (quest.npcId == npcId) return quest;
		}
		return null;
	}

    public string getNextQuestId(string npcId)
    {
        Quest quest = getNextQuest(npcId);
        return quest == null ? null : quest.questId;
    }

    public Quest getNextQuest(string npcId)
    {
		if (!questGroup.ContainsKey(npcId)) return null;
		int index = questIndex[npcId];
        return questGroup[npcId].at(index);
	}

    public QuestState? getState(string questId)
    {
        if (questId == null) return null;
        else if (!questState.ContainsKey(questId)) return null;
        return questState[questId];
    }

	public void changeState(string questId, QuestState state)
	{
        if (!questState.ContainsKey(questId)) return;
		questState[questId] = state;
	}
}

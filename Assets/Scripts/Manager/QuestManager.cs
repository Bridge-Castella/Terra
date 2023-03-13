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
        questIndex = new Dictionary<string, int>();

        // Save 데이터가 있다면 덮어씌우기
        if (GlobalContainer.contains("QuestState"))
            questState = GlobalContainer.load<Dictionary<string,QuestState>>("QuestState");
        else
            questState = new Dictionary<string, QuestState>();
    }
    #endregion

    // TODO: save quest details
    private Dictionary<string, QuestGroup> questGroup;  // Quest Group: Npc가 가지고 있는 Quest들
    private Dictionary<string, QuestState> questState;  // Quest State: Quest의 State
    private Dictionary<string, int> questIndex;         // Quest Index: Npc의 현재 Quest index

    public delegate void QuestCallbackT(Quest quest);
    public QuestCallbackT onQuestStart;

    // Quest Group 추가
    public void add(string npcId, QuestGroup questGroup)
    {
        this.questGroup.Add(npcId, questGroup);

        // Quest index 초기화
        if (!questIndex.ContainsKey(npcId))
            questIndex.Add(npcId, 0);
    }

    // Quest Group 삭제
    public void delete(string npcId)
    {
        questGroup.Remove(npcId);
    }

    // Quest State 추가
    public bool add(string questId, QuestState state)
    {
        if (questState.ContainsKey(questId))
            return false;

        questState.Add(questId, state);
        return true;
    }

    // Quest 시작: npc 이름, quest 이름
    public void StartQuest(string npcId, string questId)
    {
        // Quest object 탐색
        Quest quest = questGroup[npcId].find(questId);
		if (quest == null)
        {
            Debug.Log("QuestManager : Quest not found");
            return;
        }

        // Quest state 탐색
		QuestState state = questState[questId];
		if (state != QuestState.Null)
            return;

        // 초기화를 가지는 Start
        StartQuest(quest, true);
    }

    // Quest 시작: quest 객체, 초기화 유무
    public void StartQuest(Quest quest, bool initialize)
    {
        // Quest 상태 update
        questState[quest.questId] = QuestState.Doing;
        quest.gameObject.SetActive(true);

        // Quest destroy 방지
        quest.gameObject.transform.parent = transform;

        if (initialize)
        {
            // Quest 초기화
            quest.startQuest();
            if (onQuestStart != null)
                onQuestStart.Invoke(quest);
        }
        else
            // Quest 상태 업데이트만
            quest.updateStatus();
    }

    // Quest 포기
    public void StopQuest(string npcId)
    {
        // 활성화된 quest 중에 탐색
		Quest quest = getActiveQuestByNpc(npcId);
		if (quest == null)
		{
			Debug.Log("QuestManager : Quest not found");
			return;
		}

        // Quest 상태 update
        questState[quest.questId] = QuestState.Failed;

        // Quest 객체 destroy
        Destroy(quest.gameObject);

        // 다음 quest로 넘기기
        questIndex[npcId] += 1;
	}

    // Quest 성공
    public void SucceedQuest(string npcId)
    {
        // 활성화된 quest 중에 탐색
        Quest quest = getActiveQuestByNpc(npcId);
		if (quest == null)
		{
			Debug.Log("QuestManager : Quest not found");
			return;
		}

        // Update quest state
		questState[quest.questId] = QuestState.Completed;

        // Destroy quest object
        Destroy(quest.gameObject);

        // 다음 quest로 넘기기
		questIndex[npcId] += 1;
	}

    // Get active quests
    public List<Quest> getActiveQuests()
    {
        List<Quest> questList = new List<Quest>();
        foreach (Quest quest in GetComponentsInChildren<Quest>())
        {
            questList.Add(quest);
        }
        return questList;
    }

    // Get active quest by quest id
    public Quest getActiveQuest(string questId)
    {
		foreach (Quest quest in GetComponentsInChildren<Quest>())
		{
            if (quest.questId == questId)
                return quest;
		}
        return null;
	}

    // Get active quest by npc id
    // Only one quest for each npc can be activated
    public Quest getActiveQuestByNpc(string npcId)
    {
		foreach (Quest quest in GetComponentsInChildren<Quest>())
		{
			if (quest.npcId == npcId) return quest;
		}
		return null;
	}

    // Get next quest id by npc id
    public string getNextQuestId(string npcId)
    {
        Quest quest = getNextQuest(npcId);
        return quest == null ? null : quest.questId;
    }

    // Get next quest by npc id
    public Quest getNextQuest(string npcId)
    {
		if (!questGroup.ContainsKey(npcId)) return null;
		int index = questIndex[npcId];
        return questGroup[npcId].at(index);
	}

    // Get quest state
    public QuestState? getState(string questId)
    {
        if (questId == null)
            return null;
        else if (!questState.ContainsKey(questId))
            return null;
        return questState[questId];
    }

    // Change state
	public void changeState(string questId, QuestState state)
	{
        if (!questState.ContainsKey(questId)) return;
		questState[questId] = state;
	}

    // Get all states
    public Dictionary<string, QuestState> getState()
    {
        return questState;
    }
}

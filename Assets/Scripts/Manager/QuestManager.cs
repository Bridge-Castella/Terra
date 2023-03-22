using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    private static QuestManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    // TODO: save quest details
    public static Dictionary<string, QuestGroup> group = new Dictionary<string, QuestGroup>();
    public static Dictionary<string, QuestState> state = new Dictionary<string, QuestState>();
    private static Dictionary<string, int> index = new Dictionary<string, int>();

    public delegate void QuestCallbackT(Quest quest);
    private QuestCallbackT onQuestStart;

    // Quest Group 추가
    public static void add(string npcId, QuestGroup questGroup)
    {
        group.Add(npcId, questGroup);

        // Quest index 초기화
        if (!index.ContainsKey(npcId))
            index.Add(npcId, 0);
    }

    // Quest Group 삭제
    public static void delete(string npcId)
    {
        group.Remove(npcId);
    }

    // Quest State 추가
    public static bool add(string questId, QuestState questState)
    {
        if (state.ContainsKey(questId))
            return false;

        state.Add(questId, questState);
        return true;
    }

    // Quest 시작: npc 이름, quest 이름
    public static void StartQuest(string npcId, string questId)
    {
        // Quest object 탐색
        Quest quest = group[npcId].find(questId);
		if (quest == null)
        {
            Debug.Log("QuestManager : Quest not found");
            return;
        }

        // Quest state 탐색
		QuestState questState = state[questId];
		if (questState != QuestState.Null)
            return;

        // 초기화를 가지는 Start
        StartQuest(quest, true);
    }

    // Quest 시작: quest 객체, 초기화 유무
    public static void StartQuest(Quest quest, bool initialize)
    {
        // Quest 상태 update
        state[quest.questId] = QuestState.Doing;
        quest.gameObject.SetActive(true);

        // Quest destroy 방지
        quest.gameObject.transform.parent = instance.transform;

        if (initialize)
        {
            // Quest 초기화
            quest.startQuest();
            if (instance.onQuestStart != null)
                instance.onQuestStart.Invoke(quest);
        }
        else
            // Quest 상태 업데이트만
            quest.updateStatus();
    }

    // Quest 포기
    public static void StopQuest(string npcId)
    {
        // 활성화된 quest 중에 탐색
		Quest quest = getActiveQuestByNpc(npcId);
		if (quest == null)
		{
			Debug.Log("QuestManager : Quest not found");
			return;
		}

        // Quest 상태 update
        state[quest.questId] = QuestState.Failed;

        // Quest 객체 destroy
        Destroy(quest.gameObject);

        // 다음 quest로 넘기기
        index[npcId] += 1;
	}

    // Quest 성공
    public static void SucceedQuest(string npcId)
    {
        // 활성화된 quest 중에 탐색
        Quest quest = getActiveQuestByNpc(npcId);
		if (quest == null)
		{
			Debug.Log("QuestManager : Quest not found");
			return;
		}

        // Update quest state
		state[quest.questId] = QuestState.Completed;

        // Destroy quest object
        Destroy(quest.gameObject);

        // 다음 quest로 넘기기
		index[npcId] += 1;
	}

    // Get active quests
    public static List<Quest> getActiveQuests()
    {
        List<Quest> questList = new List<Quest>();
        foreach (Quest quest in instance.GetComponentsInChildren<Quest>())
        {
            questList.Add(quest);
        }
        return questList;
    }

    // Get active quest by quest id
    public static Quest getActiveQuest(string questId)
    {
		foreach (Quest quest in instance.GetComponentsInChildren<Quest>())
		{
            if (quest.questId == questId)
                return quest;
		}
        return null;
	}

    // Get active quest by npc id
    // Only one quest for each npc can be activated
    public static Quest getActiveQuestByNpc(string npcId)
    {
		foreach (Quest quest in instance.GetComponentsInChildren<Quest>())
		{
			if (quest.npcId == npcId) return quest;
		}
		return null;
	}

    // Get next quest id by npc id
    public static string getNextQuestId(string npcId)
    {
        Quest quest = getNextQuest(npcId);
        return quest == null ? null : quest.questId;
    }

    // Get next quest by npc id
    public static Quest getNextQuest(string npcId)
    {
		if (!group.ContainsKey(npcId)) return null;
		int questIndex = index[npcId];
        return group[npcId].at(questIndex);
	}

    // Get quest state
    public static QuestState? getState(string questId)
    {
        if (questId == null)
            return null;
        else if (!state.ContainsKey(questId))
            return null;
        return state[questId];
    }

    // Change state
	public static void changeState(string questId, QuestState questState)
	{
        if (!state.ContainsKey(questId)) return;
		state[questId] = questState;
	}

    public static void submitOnQuestStart(QuestCallbackT callback)
    {
        instance.onQuestStart += callback;
    }

    [System.Serializable]
    public struct QuestData
    {
        public Dictionary<string, QuestState> state;
        public Dictionary<string, int[]> substate;
    }

    public static QuestData saveData()
    {
        QuestData data = new QuestData();

        // save quest state
        data.state = state;
        data.substate = new Dictionary<string, int[]>();

        // save quest substate
        foreach (var quest in getActiveQuests())
            data.substate[quest.questId] = quest.saveData();

        return data;
    }

    public static void loadData(QuestData data)
    {
        state = data.state;
        GlobalContainer.store("questData", data.substate);
    }
}

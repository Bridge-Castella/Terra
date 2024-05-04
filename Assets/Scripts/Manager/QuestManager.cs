using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    private static QuestManager instance {get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private static Dictionary<string, QuestState> state;
    private static Dictionary<string, QuestGroup> group;
    private static Dictionary<string, int> index;

    public delegate void QuestCallbackT(Quest quest);
    private QuestCallbackT onQuestStart;

    [System.Serializable]
    public struct Save
    {
        public Dictionary<string, QuestState> state;
        public Dictionary<string, Quest.Save> data;
    }

    private void Start()
    {
        if (!GlobalContainer.contains("questData"))
        {
            resetData();
            return;
        }

        Dictionary<string, Quest.Save> data =
            GlobalContainer.load<Dictionary<string, Quest.Save>>("questData");

        foreach (var questData in data)
        {
            GameObject questObj = new GameObject(questData.Key);
            questObj.transform.SetParent(instance.transform);
            Quest quest = questObj.AddComponent<Quest>();
            quest.questId = questData.Key;
            quest.loadData(questData.Value);
        }
    }

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
        if (quest == null)
        {
            Debug.LogError("ERROR: Quest is not assigned");
            return;
        }

        // Quest 상태 update
        state[quest.questId] = QuestState.Doing;

        // load saved data
        Quest saved = getActiveQuest(quest.questId);

        if (initialize || saved == null)
        {
            // Quest 초기화
            quest.startQuest();
            if (instance.onQuestStart != null)
                instance.onQuestStart.Invoke(quest);

            // Quest 아이템 활성화
            for (int i = 0; i < quest.transform.childCount; i++)
                quest.transform.GetChild(i).gameObject.SetActive(true);
        }
        else
        {
            quest.loadData(saved.data);

            // Quest 상태 업데이트만
            quest.updateStatus();

            // 임시 quest object 삭제
            Destroy(saved.gameObject);
        }

        // Quest destroy 방지
        quest.transform.SetParent(instance.transform);
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


        GNBCanvas.instance.ShowToastPopup(quest.message);
        quest.questGroup.OnReward();
        InGameAudio.Post(InGameAudio.Instance.inGame_NPC_succ);
	}

    // Get active quests
    public static Quest[] getActiveQuests()
    {
        return instance.transform.GetComponentsInChildren<Quest>();
    }

    // Get active quest by quest id
    public static Quest getActiveQuest(string questId)
    {
        foreach (Quest quest in getActiveQuests())
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
        foreach (Quest quest in getActiveQuests())
        {
            if (quest.data.npcId == npcId)
                return quest;
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
		if (!group.ContainsKey(npcId))
            return null;
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
        if (!state.ContainsKey(questId))
            return;
		state[questId] = questState;
	}

    public static void submitOnQuestStart(QuestCallbackT callback)
    {
        instance.onQuestStart += callback;
    }

    public static Save saveData()
    {
        Save save = new Save();
        save.state = state;
        save.data = new Dictionary<string, Quest.Save>();

        foreach (var quest in getActiveQuests())
            save.data[quest.questId] = quest.saveData();

        return save;
    }

    public static void loadData(Save saved)
    {
        state = saved.state;
        group = new Dictionary<string, QuestGroup>();
        index = new Dictionary<string, int>();

        if (saved.data == null)
            return;

        GlobalContainer.store("questData", saved.data);
    }

    public static void resetData()
    {
        state = new Dictionary<string, QuestState>();
        group = new Dictionary<string, QuestGroup>();
        index = new Dictionary<string, int>();
    }
}

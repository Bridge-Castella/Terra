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
		quests = new Dictionary<string, QuestGroup>();
    }
    #endregion

    //public GameObject questUI;    
    private Dictionary<string, QuestGroup> quests;


    public void add(string npcId, QuestGroup questGroup)
    {
        if (quests.ContainsKey(npcId)) return;
        quests.Add(npcId, questGroup);
    } 

    //TODO: 다이얼로그에서 퀘스트 아이디를 받아와야 할듯
    public void StartQuest(string npcId, string questId = null)
    {
        Quest quest = getQuest(npcId, questId);
        if (quest == null) return;

        quest.gameObject.SetActive(true);
		quest.startQuest();
		//questUI.SetActive(true);
    }

    public void StopQuest(string npcId, string questId = null)
    {
		Quest quest = getQuest(npcId, questId);
		if (quest == null) return;

		quest.gameObject.SetActive(false);
		quest.stopQuest();
        quests[npcId].moveToNextQuest();
		//questUI.SetActive(false);
	}

    public void SucceedQuest(string npcId, string questId = null)
    {
		Quest quest = getQuest(npcId, questId);
		if (quest == null) return;

		quest.gameObject.SetActive(false);
		quest.successQuest();
        quests[npcId].moveToNextQuest();
		//questUI.SetActive(false);
    }

    #nullable enable
    public Quest? getQuest(string npcId, string? questId = null)
    {
        if (questId == null)
        {
            if (quests.ContainsKey(npcId))
                return quests[npcId].current;
            return null;
        }
        return quests[npcId].find(questId);
    }
    #nullable disable

    public List<(string, Quest)> getAllQuests(bool active = true)
    {
        List<(string npcId, Quest quest)> questList = new List<(string, Quest)>();
        foreach (KeyValuePair<string, QuestGroup> ele in quests)
        {
            if (ele.Value.current == null) continue;
            else if (active && ele.Value.current.state != QuestState.Doing) continue;
            questList.Add((npcId: ele.Key, quest: ele.Value.current));
        }
        return questList;
    }
}

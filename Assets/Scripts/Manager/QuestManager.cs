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
		questGroups = new Dictionary<string, QuestGroup>();
    }
    #endregion

    public GameObject questUI;    
    private Dictionary<string, QuestGroup> questGroups;


    public void add(string npcId, QuestGroup questGroup)
    {
        questGroups.Add(npcId, questGroup);
    } 

    //TODO: 다이얼로그에서 퀘스트 아이디를 받아와야 할듯
    public void StartQuest(string npcId, string questId = null)
    {
        Quest quest;
        if (questId == null) quest = (Quest)getQuest(npcId);
        else quest = (Quest)getQuest(npcId, questId);

        quest.gameObject.SetActive(true);
        quest.startQuest();
        questUI.SetActive(true);
    }

    public void StopQuest(string npcId, string questId = null)
    {
		Quest quest;
		if (questId == null) quest = (Quest)getQuest(npcId);
		else quest = (Quest)getQuest(npcId, questId);

		quest.stopQuest();
		quest.gameObject.SetActive(false);
		questUI.SetActive(false);
	}

    public void SucceedQuest(string npcId, string questId = null)
    {
		Quest quest;
		if (questId == null) quest = (Quest)getQuest(npcId);
		else quest = (Quest)getQuest(npcId, questId);

		quest.successQuest();
		quest.gameObject.SetActive(false);
        questGroups[npcId].delete();
		questUI.SetActive(false);
    }

    #nullable enable
    public Quest? getQuest(string npcId)
    {
        if (questGroups.ContainsKey(npcId))
            return questGroups[npcId].current;
        return null;
    }

    public Quest? getQuest(string npcId, string questId)
    {
        return questGroups[npcId].find(questId);
    }
    #nullable disable
}

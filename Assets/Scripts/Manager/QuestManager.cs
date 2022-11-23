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
    }
    #endregion

    //퀘스트 중인지 확인
    [HideInInspector] public bool isQuesting = false;
    [HideInInspector] public bool isComplete = false;
    [HideInInspector] public bool isFailed = false;

    public GameObject questUI;
    public List<QuestGroup> questItemGroups;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    public TextMeshProUGUI questStatusText;

    public int curQuestItemNum = 0;
    public int questItemTotalNum = 5;

    //TODO: 다이얼로그에서 퀘스트 아이디를 받아와야 할듯
    public void StartQuest(string questID)
    {
        curQuestItemNum = 0;
        isQuesting = true;
        questUI.SetActive(true);

        foreach(QuestGroup questGroup in questItemGroups)
        {
            if(questGroup.questID == questID)
            {
                questGroup.gameObject.SetActive(true);

                questItemTotalNum = questGroup.questItemTotalNum;
                questTitleText.text = questGroup.questTitle;
                questDescText.text = questGroup.questDesc;
                questStatusText.text = string.Format("종이: {0} / {1}", curQuestItemNum, questGroup.questItemTotalNum);
            }                
        }
    }

    public void StopQuest()
    {
        isQuesting = false;
        isFailed = true;
        questUI.SetActive(false);
    }

    public void SucceedQuest()
    {
        isQuesting = false;
        questUI.SetActive(false);
    }
}

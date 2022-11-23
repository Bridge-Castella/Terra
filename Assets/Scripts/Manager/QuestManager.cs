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

    //����Ʈ ������ Ȯ��
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

    //TODO: ���̾�α׿��� ����Ʈ ���̵� �޾ƿ;� �ҵ�
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
                questStatusText.text = string.Format("����: {0} / {1}", curQuestItemNum, questGroup.questItemTotalNum);
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

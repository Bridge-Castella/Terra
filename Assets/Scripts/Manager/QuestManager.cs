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
    public GameObject questItemGroup;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    public TextMeshProUGUI questStatusText;

    public int questItemNum = 0;
    public int questItemTotalNum = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: 다이얼로그에서 퀘스트 아이디를 받아와야 할듯
    public void StartQuest()
    {
        isQuesting = true;
        questUI.SetActive(true);
        questItemGroup.SetActive(true);

        //임시
        questTitleText.text = "라토에게 종이를 가져다주기";
        questDescText.text = "라토가 잃어버린 종이에는 숲을 방어하는 방법이 적혀져 있다고 한다.";
        questStatusText.text = string.Format("종이: {0} / {1}", questItemNum, questItemTotalNum);
    }

    public void StopQuest()
    {
        isQuesting = false;
        isFailed = true;
        questUI.SetActive(false);
    }
}

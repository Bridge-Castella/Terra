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

    //TODO: ���̾�α׿��� ����Ʈ ���̵� �޾ƿ;� �ҵ�
    public void StartQuest()
    {
        isQuesting = true;
        questUI.SetActive(true);
        questItemGroup.SetActive(true);

        //�ӽ�
        questTitleText.text = "���信�� ���̸� �������ֱ�";
        questDescText.text = "���䰡 �Ҿ���� ���̿��� ���� ����ϴ� ����� ������ �ִٰ� �Ѵ�.";
        questStatusText.text = string.Format("����: {0} / {1}", questItemNum, questItemTotalNum);
    }

    public void StopQuest()
    {
        isQuesting = false;
        isFailed = true;
        questUI.SetActive(false);
    }
}

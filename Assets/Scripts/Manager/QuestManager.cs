using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isQuesting = false;
    public bool isComplete = false;
    public bool isFalied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartQuest()
    {
        isQuesting = true;
    }

    public void StopQuest()
    {
        isQuesting = false;
        isFalied = true;
    }
}

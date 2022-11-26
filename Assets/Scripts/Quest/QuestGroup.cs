using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGroup : MonoBehaviour
{
    public string questID;
    public int questItemTotalNum;
    public string questTitle;
    public string questDesc;

    private void Start()
    {
        //퀘스트 테이블 만들어지면 questID로 접근해서 값 가져오기..
        questItemTotalNum = this.transform.childCount;
    }
}

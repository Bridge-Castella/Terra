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
        //����Ʈ ���̺� ��������� questID�� �����ؼ� �� ��������..
        questItemTotalNum = this.transform.childCount;
    }
}

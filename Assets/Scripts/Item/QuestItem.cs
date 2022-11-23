using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    public override void GetQuestItem()
    {
        QuestManager.instance.curQuestItemNum++;
        QuestManager.instance.questStatusText.text = string.Format("종이: {0} / {1}", QuestManager.instance.curQuestItemNum, QuestManager.instance.questItemTotalNum);
        //퀘스트아이템 다 모음
        if (QuestManager.instance.curQuestItemNum >= QuestManager.instance.questItemTotalNum)
        {
            QuestManager.instance.isComplete = true;
            QuestManager.instance.isQuesting = false;
        }

        bool wasPickedUp = Inventory.instance.Add(this, 1);

        if (wasPickedUp)
        {
            gameObject.SetActive(false);
        }
    }
}

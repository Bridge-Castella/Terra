using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    public override void GetQuestItem()
    {
        QuestManager.instance.questItemNum++;
        QuestManager.instance.questStatusText.text = string.Format("����: {0} / {1}", QuestManager.instance.questItemNum, QuestManager.instance.questItemTotalNum);
        //����Ʈ������ �� ����
        if (QuestManager.instance.questItemNum == QuestManager.instance.questItemTotalNum)
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

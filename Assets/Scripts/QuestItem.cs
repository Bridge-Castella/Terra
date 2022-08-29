using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            QuestManager.instance.questItemNum ++;
            QuestManager.instance.questStatusText.text = string.Format("¡æ¿Ã: {0} / 5", QuestManager.instance.questItemNum);
            if(QuestManager.instance.questItemNum == QuestManager.instance.questItemTotalNum)
            {
                QuestManager.instance.isComplete = true;
                QuestManager.instance.isQuesting = false;
            }
                
            Destroy(this.gameObject);
        }
    }
}

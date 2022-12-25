using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
	public override void GetQuestItem(Collider2D collision)
    {
        Quest quest = transform.parent.GetComponent<Quest>();
        quest.getItem(collision);
        quest.updateStatus();
        
        bool wasPickedUp = Inventory.instance.Add(this, 1);

        if (wasPickedUp)
        {
            gameObject.SetActive(false);
        }
    }
}

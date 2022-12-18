using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
	public override void GetQuestItem(Collider2D collision)
    {
        Quest quest = transform.parent.parent.GetComponent<QuestGroup>().current;
        quest.collideItem(collision);
        quest.updateState();

        bool wasPickedUp = Inventory.instance.Add(this, 1);

        if (wasPickedUp)
        {
            gameObject.SetActive(false);
        }
    }
}

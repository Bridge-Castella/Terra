using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    [SerializeField]
    private string storyId;
    [SerializeField]
    private string npcId;

    public override void GetQuestItem(Collider2D collision)
    {
        Quest quest = transform.parent.GetComponent<Quest>();
        quest.getItem(collision);
        quest.updateStatus();
        
        bool wasPickedUp = Inventory.instance.Add(this, 1);
        GNBCanvas.instance.ShowToastPopup(quest.data.status);

        if (quest.state == QuestState.Succeeded)
        {
            GNBCanvas.instance.DialoguePanel.GetComponent<Dialogue>().QuestDialogue("quest", storyId, npcId);
        }

        if (wasPickedUp)
        {
            gameObject.SetActive(false);
        }
    }
}

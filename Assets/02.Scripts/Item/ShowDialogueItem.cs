using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialogueItem : MonoBehaviour
{
    private bool isShowed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isShowed)
            {
                GNBCanvas.instance.DialoguePanel.GetComponent<Dialogue>().QuestDialogue("quest", "c_quest_3", "c_quest_3_1");
                isShowed = true;
            }
            else
                return;
        }
    }
}

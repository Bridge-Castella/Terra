using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestGroup : MonoBehaviour
{
	[Header("Quest Objects")]
	[SerializeField] List<GameObject> questObject;
	[SerializeField]
	private GameObject rewardObject;

	// List of quests
	private List<Quest> questList;

	public UnityAction RewardAction;

	private string questId;

	private void Start()
	{
		// Get npc id
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;

		// Add this QuestGroup to QuestManager
		QuestManager.add(npcId, this);

        // Create a quest list from quest object list
        questList = new List<Quest>();
		foreach (GameObject questEle in questObject)
		{
			if (questEle == null)
			{
				Debug.LogError("ERROR: Quest list contains null object. " +
					"Index of " + questObject.IndexOf(questEle) + " in " +
					gameObject.name + "/QuestGroup/QuestObject");
				continue;
			}

			// Get quest
			Quest quest = questEle.GetComponent<Quest>();
			quest.questGroup = this;
			this.questId = quest.questId;

			QuestState? state = QuestManager.getState(quest.questId);

			questList.Add(quest);

			if (state == null || state == QuestState.Null)
			{
				// initialize quest
				quest.init(npcId);

				// submit quest to manager
				QuestManager.add(quest.questId, QuestState.Null);

				return;
			}

			switch (state.Value)
			{
				case QuestState.Doing:
					QuestManager.StartQuest(quest, false);
					break;

				case QuestState.Failed:
				case QuestState.Succeeded:
					quest.gameObject.SetActive(false);
					break;
			}
		}
	}

	// Destory current group
	private void OnDestroy()
	{
		string npcId = gameObject.GetComponent<NpcAction>().npc_diff_id;
		QuestManager.delete(npcId);
	}

	public void OnReward()
	{
        SaveManager.SaveGame();
        switch (questId)
		{
			case "quest_1":
				// TODO: implement double jump enabled sound
				Inventory.instance.RemoveAll(item => item.itemId.Equals("quest_1"));
                ControlManager.instance.player.GetComponent<PlayerAbilityTracker>().canDoubleJump = true;
				break;
			case "quest_2":
                //날개 아이템 활성화
                rewardObject.SetActive(true);
                break;
        }
	}

	// Find quest on quest list
	public Quest find(string questId)
	{
		foreach (Quest quest in questList)
		{
			if (quest.questId == questId)
				return quest;
		}
		return null;
	}

	public Quest at(int index)
	{
		if (questList.Count <= index)
			return null;
		return questList[index];
	}
}

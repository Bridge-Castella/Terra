using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState {
    Null = 0,
    Doing,
    Failed,
    Succeeded,
    Completed
};

public class Quest : MonoBehaviour
{
    [System.Serializable]
    public struct Save
    {
        public string npcId;                         
        public string title;
        public string description;
        public string status;
        public string portraitId;
        public string itemId;
        public int[] substate;
    }

    public string questId;                                  // quest name
    [HideInInspector] public Save data;
	[HideInInspector] public Sprite portrait;               // display npc portrait
	[HideInInspector] public Sprite itemIcon;               // display item icon

    protected virtual bool didSuccess() { return false; }   // check if quest has been succeeded, abstract function
    protected virtual void onStart() { }                    // called on starting of the quest, abstract function
    protected virtual void onChange() { }                   // called when quest state is changed, abstract function

    public delegate void CallbackT();
    private CallbackT OnChangeStatusCallback;

    // Initialize distplay variables
    public void init(string npcId)
    {
        Quest.Save data = TableData.instance.GetQuestData(questId);
        this.data = data;
        this.data.npcId = npcId;

        portrait = TableData.instance.GetPortrait(data.portraitId);
        itemIcon = TableData.instance.GetItemSprite(data.itemId);
	}

	public void updateStatus()
    {
        onChange();
		if (didSuccess()) onSuccess();
		if (OnChangeStatusCallback != null)
            OnChangeStatusCallback.Invoke();
    }

    public void submitCallback(CallbackT callback)
    {
        OnChangeStatusCallback += callback;
    }

    public void onSuccess()
    {
		QuestManager.changeState(questId, QuestState.Succeeded);
	}

	public void startQuest()
	{
		onStart();
		updateStatus();
	}

	protected virtual void getItemCallback(Collider2D item) { }
	public void getItem(Collider2D item)
    {
        getItemCallback(item);
        updateStatus();
    }

	public QuestState state { get { return _state(); } }
	private QuestState _state()
    {
        return (QuestState)QuestManager.getState(questId);
    }

    // save current state of quest
    public virtual Save saveData()
    {
        return this.data;
    }

    public virtual void loadData(Save data)
    {
        this.data = data;

        portrait = TableData.instance.GetPortrait(data.portraitId);
        itemIcon = TableData.instance.GetItemSprite(data.itemId);
    }
}

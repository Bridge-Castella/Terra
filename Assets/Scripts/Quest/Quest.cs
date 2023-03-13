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

public abstract class Quest : MonoBehaviour
{
    public string questId;                              // quest name
	[HideInInspector] public string npcId;              // npc name
	[HideInInspector] public string title;              // display title
	[HideInInspector] public string description;        // display description
	[HideInInspector] public string status;             // display status

	[HideInInspector] public Sprite portrait;           // display npc portrait
	[HideInInspector] public Sprite itemIcon;           // display item icon

    protected abstract bool didSuccess();               // check if quest has been succeeded, abstract function
	protected abstract void onStart();                  // called on starting of the quest, abstract function
    protected abstract void onChange();                 // called when quest state is changed, abstract function

    public delegate void CallbackT();
    private CallbackT OnChangeStatusCallback;

    // Initialize distplay variables
    public void init(string npcId)
    {
        this.npcId = npcId;
        TableData.QuestData data = TableData.instance.GetQuestData(questId);
        this.title = data.title;
        this.description = data.description;

        portrait = TableData.instance.GetPortrait(data.portrait_id);
        itemIcon = TableData.instance.GetItemSprite(data.item_id);
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
		QuestManager.instance.changeState(questId, QuestState.Succeeded);
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
        return (QuestState)QuestManager.instance.getState(questId);
    }
}

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
    public string questId;
	[HideInInspector] public string npcId;
	[HideInInspector] public string title;
	[HideInInspector] public string description;
	[HideInInspector] public string status;

	[HideInInspector] public Sprite portrait;
	[HideInInspector] public Sprite itemIcon;

    protected abstract bool didSuccess();
	protected abstract void onStart();
    protected abstract void onChange();

    public delegate void CallbackT();
    private CallbackT OnChangeStatusCallback;

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

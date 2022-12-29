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
    public string npcId;
    public string title;
    public string description;
    public string status;

    public Sprite portrait;
    public Sprite itemIcon;

    protected abstract bool didSuccess();
	protected abstract void start();
    protected abstract void onChange();

    public delegate void CallbackT();
    private CallbackT OnChangeStatusCallback;

    public void updateStatus()
    {
        onChange();
		if (didSuccess()) success();
		if (OnChangeStatusCallback != null)
            OnChangeStatusCallback.Invoke();
    }

    public void submitCallback(CallbackT callback)
    {
        OnChangeStatusCallback += callback;
    }

    public void success()
    {
		QuestManager.instance.changeState(questId, QuestState.Succeeded);
	}

	public void startQuest()
	{
		start();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState {
    Null = 0,
    Doing,
    Failed,
    Succeeded
};

public abstract class Quest : MonoBehaviour
{
    public string questId;
    public string title;
    public string description;
    public string statusStr;

    public QuestState state { get { return _state; } }
    private QuestState _state = QuestState.Null;


    protected abstract bool didSuccess();
	protected abstract void start();
    protected abstract void stop();
	protected abstract void success();
    protected abstract void update();

    public virtual void collideItem(Collider2D item) { }


	protected virtual void Start()
	{
        transform.parent.GetComponent<QuestGroup>().add((Quest)this);
	}


	public bool testQuest()
    {
        return didSuccess();
    }

    public void updateState()
    {
        this.update();
        if (didSuccess())
            successQuest();
    }

    public void resetState()
    {
        _state = QuestState.Null;
    }

    public void startQuest()
    {
        if (testQuest())
        {
            successQuest();
            return;
        }
		_state = QuestState.Doing;
        this.start();
    }

    public void stopQuest()
    {
        if (testQuest())
        {
            successQuest();
            return;
        }
		_state = QuestState.Failed;
        this.stop();
    }

    public void successQuest()
    {
		_state = QuestState.Succeeded;
        this.success();
    }
}

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
    public string portraitId;
    public string title;
    public string description;
    public string statusStr;

    public QuestState state { get { return _state; } }
    private QuestState _state = QuestState.Null;


    protected abstract bool didSuccess();
    protected abstract void start();
    protected abstract void update();
    protected abstract void reset();

    protected virtual void stop() { }
    protected virtual void success() { }
    public virtual void getItem(Collider2D item) { }


	public bool testQuest()
    {
        if (state == QuestState.Null) return false;
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
        reset();
        _state = QuestState.Null;
    }

    public void startQuest()
    {
		_state = QuestState.Doing;
        reset();
        start();
        update();
    }

    public void stopQuest()
    {
		_state = QuestState.Failed;
        stop();
        reset();
    }

    public void successQuest()
    {
		_state = QuestState.Succeeded;
        success();
        reset();
    }
}

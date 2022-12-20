using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState {
    Null = 0,
    Doing,
    Failed,
    Succeeded,
    End
};

public abstract class Quest : MonoBehaviour
{
    public string questId;
    public string npcId;
    public string portraitId;
    public string title;
    public string description;
    public string statusStr;

    public abstract bool didSuccess();
    public abstract void start();
    public abstract void update();
    public abstract void reset();


    public void success()
    {
		QuestManager.instance.changeState(questId, QuestState.Succeeded);
	}

    public virtual void getItem(Collider2D item) { }

	private QuestState _state()
    {
        return (QuestState)QuestManager.instance.getState(questId);
    }

    public QuestState state { get { return _state(); } }
}

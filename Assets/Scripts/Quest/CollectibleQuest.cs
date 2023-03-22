using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleQuest : Quest
{
    protected int itemTotalNum;
    protected int currentItemNum;
    [SerializeField] private string displayName = "";

    protected override bool didSuccess()
    {
        return itemTotalNum == currentItemNum;
    }

    protected override void onStart()
    {
        itemTotalNum = this.transform.childCount;
        currentItemNum = 0;
    }

    protected override void onChange()
    {
        data.status = string.Format(displayName + ": {0} / {1}",
                                    this.currentItemNum,
                                    this.itemTotalNum);
    }

    protected override void getItemCallback(Collider2D item)
    {
        currentItemNum++;
    }

    public override Save saveData()
    {
        Save data = base.saveData();
        data.substate = new int[2];
        data.substate[0] = itemTotalNum;
        data.substate[1] = currentItemNum;
        return data;
    }

    public override void loadData(Save data)
    {
        base.loadData(data);
        itemTotalNum = data.substate[0];
        currentItemNum = data.substate[1];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleQuest : Quest
{
    protected int itemTotalNum;
    protected int currentItemNum;
    public string displayName = "";

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
        status = string.Format(displayName + ": {0} / {1}",
                               this.currentItemNum,
                               this.itemTotalNum);
    }

    protected override void getItemCallback(Collider2D item)
    {
        currentItemNum++;
    }

    public override int[] saveData()
    {
        int[] data = new int[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            var item = transform.GetChild(i).gameObject;
            data[item.transform.GetSiblingIndex()] = item.activeSelf ? 1 : 0;
        }

        return data;
    }

    public override void loadData(int[] data)
    {
        itemTotalNum = this.transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (data[i] == 0)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                currentItemNum++;
            }
        }
    }
}

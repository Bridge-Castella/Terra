using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRiche : Quest
{
	private int itemTotalNum;
	private int currentItemNum;

	public override bool didSuccess()
	{
		return itemTotalNum == currentItemNum;
	}

	public override void update()
	{
		this.statusStr = string.Format("인형: {0} / {1}",
									   this.currentItemNum,
									   this.itemTotalNum);
	}

	public override void reset()
	{
		currentItemNum = 0;
	}

	public override void start()
	{
		itemTotalNum = this.transform.childCount;
	}

	public override void getItem(Collider2D item)
	{
		currentItemNum++;
	}
}

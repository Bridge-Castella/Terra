using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRato : Quest
{
	private int itemTotalNum;
	private int currentItemNum;

	protected override bool didSuccess()
	{
		return itemTotalNum == currentItemNum;
	}

	protected override void onChange()
	{
		this.status = string.Format("종이: {0} / {1}",
									this.currentItemNum,
									this.itemTotalNum);
	}

	protected override void start()
	{
		itemTotalNum = this.transform.childCount;
		currentItemNum = 0;
	}

	protected override void getItemCallback(Collider2D item)
	{
		currentItemNum++;
	}
}

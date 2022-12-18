using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRato : Quest
{
	private int itemTotalNum;
	private int currentItemNum;

	protected override void Start()
	{
		base.Start();

		//퀘스트 테이블 만들어지면 questID로 접근해서 값 가져오기..
		itemTotalNum = this.transform.childCount;
	}

	protected override bool didSuccess()
	{
		return itemTotalNum == currentItemNum;
	}

	protected override void update()
	{
		this.statusStr = string.Format("종이: {0} / {1}",
									   this.currentItemNum,
									   this.itemTotalNum);
	}

	protected override void start()
	{
		currentItemNum = 0;
	}

	protected override void stop()
	{
		
	}

	protected override void success()
	{

	}

	public override void collideItem(Collider2D item)
	{
		currentItemNum++;
	}
}

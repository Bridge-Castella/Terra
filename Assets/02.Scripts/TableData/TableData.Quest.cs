using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    Dictionary<string, Quest.Save> questDict = new Dictionary<string, Quest.Save>();

    void QuestDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("npc_quest_table");

        for (int i = 0; i < data.Count; i++)
        {
			string quest_id = data[i]["quest_id"].ToString();
            if (mainDataDic.ContainsKey(quest_id)) continue;

			Quest.Save questData = new Quest.Save();

            questData.title = data[i]["title"].ToString();
            questData.description = data[i]["description"].ToString();
            questData.portraitId = data[i]["portrait_id"].ToString();
            questData.itemId = data[i]["item_id"].ToString();

            questDict.Add(quest_id, questData);
        }
    }

    public Quest.Save GetQuestData(string quest_id)
    {
        return questDict[quest_id];
    }

    public Sprite GetItemSprite(string item_id)
    {
        return Resources.Load<Sprite>("items/" + item_id);
	}
}

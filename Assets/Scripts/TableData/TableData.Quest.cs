using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    public class QuestData
    {
        public string title;
        public string description;
        public string portrait_id;
        public string item_id;
    }

    Dictionary<string, QuestData> questDict = new Dictionary<string, QuestData>();

    void QuestDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("npc_quest_table");

        for (int i = 0; i < data.Count; i++)
        {
			string quest_id = data[i]["quest_id"].ToString();
            if (mainDataDic.ContainsKey(quest_id)) continue;

			QuestData questData = new QuestData();

            questData.title = data[i]["title"].ToString();
            questData.description = data[i]["description"].ToString();
            questData.portrait_id = data[i]["portrait_id"].ToString();
            questData.item_id = data[i]["item_id"].ToString();

            questDict.Add(quest_id, questData);
        }
    }

    public QuestData GetQuestData(string quest_id)
    {
        return questDict[quest_id];
    }

    public Sprite GetItemSprite(string item_id)
    {
        return Resources.Load<Sprite>("quest_items/" + item_id);
	}
}

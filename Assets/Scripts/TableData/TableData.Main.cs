using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    public class MainData
    {
        public string string_id;
        public int string_type;
        public int value_1;
        public int conv_type;
        public string answer1_string_id;
        public string answer1_connect_id;
        public string answer2_string_id;
        public string answer2_connect_id;
        public string conv_connect_id;
    }

    Dictionary<string, Dictionary<string, List<MainData>>> mainDataDic = new Dictionary<string, Dictionary<string, List<MainData>>>();

    void MainDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("main_table");

        for (int i = 0; i < data.Count; i++)
        {
            MainData mainData = new MainData();

            string story_id = data[i]["story_id"].ToString();
            string npc_id = data[i]["npc_id"].ToString();

            mainData.string_id = data[i]["string_id"].ToString();
            mainData.value_1 = int.Parse(data[i]["value_1"].ToString());
            mainData.conv_type = int.Parse(data[i]["conv_type"].ToString());
            mainData.answer1_string_id = data[i]["answer1_string_id"].ToString();
            mainData.answer1_connect_id = data[i]["answer1_connect_id"].ToString();
            mainData.answer2_string_id = data[i]["answer2_string_id"].ToString();
            mainData.answer2_connect_id = data[i]["answer2_connect_id"].ToString();
            mainData.conv_connect_id = data[i]["conv_connect_id"].ToString();

            if(!mainDataDic.ContainsKey(story_id))
            {
                mainDataDic.Add(story_id, new Dictionary<string, List<MainData>>());
                if (!mainDataDic[story_id].ContainsKey(npc_id))
                {
                    mainDataDic[story_id].Add(npc_id, new List<MainData>());
                    mainDataDic[story_id][npc_id].Add(mainData);
                }
                else
                {
                    mainDataDic[story_id][npc_id].Add(mainData);
                }
            }
            else
            {
                if (!mainDataDic[story_id].ContainsKey(npc_id))
                {
                    mainDataDic[story_id].Add(npc_id, new List<MainData>());
                    mainDataDic[story_id][npc_id].Add(mainData);
                }
                else
                {
                    mainDataDic[story_id][npc_id].Add(mainData);
                }
            }
        }
    }

    public Dictionary<string, Dictionary<string, List<MainData>>> GetMainDataDic()
    {
        return mainDataDic;
    }

    public List<MainData> GetStringIdDataList(string stroy_id, string npc_id)
    {
        List<MainData> list = mainDataDic[stroy_id][npc_id];
        return list;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    public class MainData
    {
        public string string_id;
        public int conv_type;
        public string answer1_string_id;
        public string answer1_connect_id;
        public string answer2_string_id;
        public string answer2_connect_id;
        public string conv_connect_id;
        public string quest_id;
        public string portrait_id;
    }

    Dictionary<string, Dictionary<string, Dictionary<string, List<MainData>>>> mainDataDic = new Dictionary<string, Dictionary<string, Dictionary<string, List<MainData>>>>();

    void MainDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("npc_main_table");

        for (int i = 0; i < data.Count; i++)
        {
            MainData mainData = new MainData();

            string npc_diff_id = data[i]["npc_diff_id"].ToString();
            string story_id = data[i]["story_id"].ToString();
            string npc_id = data[i]["npc_id"].ToString();

            mainData.string_id = data[i]["string_id"].ToString();
            mainData.conv_type = int.Parse(data[i]["conv_type"].ToString());
            mainData.answer1_string_id = data[i]["answer1_string_id"].ToString();
            mainData.answer1_connect_id = data[i]["answer1_connect_id"].ToString();
            mainData.answer2_string_id = data[i]["answer2_string_id"].ToString();
            mainData.answer2_connect_id = data[i]["answer2_connect_id"].ToString();
            mainData.conv_connect_id = data[i]["conv_connect_id"].ToString();
            mainData.quest_id = data[i]["quest_id"].ToString();
            mainData.portrait_id = data[i]["portrait_id"].ToString();


            if (!mainDataDic.ContainsKey(npc_diff_id))
            {
                mainDataDic.Add(npc_diff_id, new Dictionary<string, Dictionary<string, List<MainData>>>());
                if (!mainDataDic[npc_diff_id].ContainsKey(story_id))
                {
                    mainDataDic[npc_diff_id].Add(story_id, new Dictionary<string, List<MainData>>());
                    if (!mainDataDic[npc_diff_id][story_id].ContainsKey(npc_id))
                    {
                        mainDataDic[npc_diff_id][story_id].Add(npc_id, new List<MainData>());
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                    else
                    {
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                }
                else
                {
                    if (!mainDataDic[npc_diff_id][story_id].ContainsKey(npc_id))
                    {
                        mainDataDic[npc_diff_id][story_id].Add(npc_id, new List<MainData>());
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                    else
                    {
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                }
            }
            else
            {
                if (!mainDataDic[npc_diff_id].ContainsKey(story_id))
                {
                    mainDataDic[npc_diff_id].Add(story_id, new Dictionary<string, List<MainData>>());
                    if (!mainDataDic[npc_diff_id][story_id].ContainsKey(npc_id))
                    {
                        mainDataDic[npc_diff_id][story_id].Add(npc_id, new List<MainData>());
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                    else
                    {
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                }
                else
                {
                    if (!mainDataDic[npc_diff_id][story_id].ContainsKey(npc_id))
                    {
                        mainDataDic[npc_diff_id][story_id].Add(npc_id, new List<MainData>());
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                    else
                    {
                        mainDataDic[npc_diff_id][story_id][npc_id].Add(mainData);
                    }
                }
            }
        }
    }

    public Dictionary<string, Dictionary<string, List<MainData>>> GetMainDataDic(string npc_diff_id)
    {
        return mainDataDic[npc_diff_id];
    }
}

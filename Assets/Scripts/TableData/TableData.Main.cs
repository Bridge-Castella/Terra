using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    public class MainData
    {
        public string communication_id;
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
    Dictionary<string, List<MainData>> mainDataDic = new Dictionary<string, List<MainData>>();

    void MainDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("main_table");

        for (int i = 0; i < data.Count; i++)
        {
            MainData mainData = new MainData();

            mainData.communication_id = data[i]["communication_id"].ToString();
            mainData.string_id = data[i]["string_id"].ToString();
            mainData.string_type = int.Parse(data[i]["string_type"].ToString());
            mainData.value_1 = int.Parse(data[i]["value_1"].ToString());
            mainData.conv_type = int.Parse(data[i]["conv_type"].ToString());
            mainData.answer1_string_id = data[i]["answer1_string_id"].ToString();
            mainData.answer1_connect_id = data[i]["answer1_connect_id"].ToString();
            mainData.answer2_string_id = data[i]["answer2_string_id"].ToString();
            mainData.answer2_connect_id = data[i]["answer2_connect_id"].ToString();
            mainData.conv_connect_id = data[i]["conv_connect_id"].ToString();

            string npc_id = data[i]["npc_id"].ToString();
            if (!mainDataDic.ContainsKey(npc_id))
            {
                mainDataDic.Add(npc_id, new List<MainData>());
                mainDataDic[npc_id].Add(mainData);
            }
            else
            {
                mainDataDic[npc_id].Add(mainData);
            }
        }
    }

    public Dictionary<string, List<MainData>> GetMainDataDic()
    {
        return mainDataDic;
    }

    public List<MainData> GetStringIdDataList(string npc_id)
    {
        List<MainData> list = new List<MainData>();
        list = mainDataDic[npc_id];
        return list;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableData : MonoBehaviour
{
    public class PortraitData
    {
        public string portrait_name;
    }

    Dictionary<string, PortraitData> portraitDataDic = new Dictionary<string, PortraitData>();

    void CharacterDataInit()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("portrait_table");

        for (int i = 0; i < data.Count; i++)
        {
            portraitDataDic.Add(data[i]["portrait_id"].ToString(), new PortraitData());
            portraitDataDic[data[i]["portrait_id"].ToString()].portrait_name = data[i]["portrait_name"].ToString();
        }
    }

    public string GetPortraitName(string portrait_id)
    {
        return portraitDataDic[portrait_id].portrait_name;
    }

    public Sprite GetPortrait(string portrait_id)
    {
        Sprite sprite = Resources.Load<Sprite>("portraits/" + portrait_id);
        return sprite;
    }
}

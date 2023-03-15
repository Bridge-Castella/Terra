using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveManager
{
    [System.Serializable]
    public struct SaveData
    {
        public float checkPoint_x;
        public float checkPoint_y;
        public float checkPoint_z;

        // TODO: inventory 저장

        public int playerHeart;
        public MapManager.MapData mapData;
        public QuestManager.QuestData questData;
    }

    public static void SaveGame()
    {
        SaveData data = new SaveData();
        Vector3 checkPoint = ControlManager.instance.startPoint;
        data.checkPoint_x = checkPoint.x;
        data.checkPoint_y = checkPoint.y;
        data.checkPoint_z = checkPoint.z;
        data.playerHeart = HeartManager.instance.heartNum;

        data.mapData = MapManager.saveData();
        data.questData = QuestManager.saveData();

        try
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/data.sav");
            binary.Serialize(file, data);
            file.Close();
        }
        catch (Exception exception) when (exception is IOException ||
                                          exception is SerializationException)
        {
            Debug.Log("파일을 저장할 수 없습니다");
        }
        catch (Exception)
        {
            Debug.Log("알 수 없는 오류가 발생하여 게임을 저장할 수 없습니다");
        }
    }

    public static SaveData? LoadGame()
    {
        BinaryFormatter binary = new BinaryFormatter();
        FileStream file;
        SaveData data;

        try
        {
            file = File.Open(Application.persistentDataPath + "/data.sav", FileMode.Open);
            data = (SaveData)binary.Deserialize(file);
            file.Close();
        }
        catch (Exception exception) when (exception is FileNotFoundException)
        {
            Debug.Log("저장 파일이 존재하지 않습니다");
            return null;
        }
        catch (Exception exception) when (exception is IOException ||
                                          exception is SerializationException)
        {
            Debug.Log("저장 파일이 손상되었습니다");
            return null;
        }
        catch (Exception)
        {
            Debug.Log("알 수 없는 오류가 발생하여 게임을 불러올 수 없습니다");
            return null;
        }

        Vector3 checkPoint = new Vector3();
        checkPoint.x = data.checkPoint_x;
        checkPoint.y = data.checkPoint_y;
        checkPoint.z = data.checkPoint_z;

        MapManager.loadData(data.mapData);
        QuestManager.loadData(data.questData);
        GlobalContainer.store("StartPos", checkPoint);
        GlobalContainer.store("Heart", data.playerHeart);

        return data;
    }
}

using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveManager
{
    [System.Serializable]
    public struct SerializableVec
    {
        public float x;
        public float y;
        public float z;

        public static SerializableVec Convert(Vector3 vec)
        {
            return new SerializableVec { x = vec.x, y = vec.y, z = vec.z };
        }

        public static Vector3 ToVec3(SerializableVec vec)
        {
            return new Vector3 { x = vec.x, y = vec.y, z = vec.z };
        }
    }

    [System.Serializable]
    public struct SaveData
    {
        public SerializableVec checkPoint;
        public int playerHeart;

        public MapManager.Save mapData;
        public QuestManager.Save questData;
        public Inventory.Save inventoryData;
    }

    public static void SaveGame()
    {
        SaveData data = new SaveData
        {
            checkPoint = SerializableVec.Convert(ControlManager.instance.startPoint),
            playerHeart = HeartManager.instance.heartNum,
            mapData = MapManager.SaveData(),
            questData = QuestManager.saveData(),
            inventoryData = Inventory.instance.SaveData()
        };

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

        MapManager.LoadData(data.mapData);
        QuestManager.loadData(data.questData);
        GlobalContainer.store("inventory", data.inventoryData);
        GlobalContainer.store("StartPos", SerializableVec.ToVec3(data.checkPoint));
        GlobalContainer.store("Heart", data.playerHeart);

        return data;
    }
}

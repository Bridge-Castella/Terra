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

        public static implicit operator SerializableVec(Vector3 vec)
        {
            return new SerializableVec { x = vec.x, y = vec.y, z = vec.z };
        }

        public static implicit operator Vector3(SerializableVec vec)
        {
            return new Vector3 { x = vec.x, y = vec.y, z = vec.z };
        }
    }

    [System.Serializable]
    public struct SaveData
    {
        public SerializableVec checkPoint;
        public int playerHeart;
        public bool[] homePhotoInitialized;
        public bool[] LadderInitialized;

        public MapManager.Save mapData;
        public QuestManager.Save questData;
        public Inventory.Save inventoryData;
        public PlayerAbilityTracker.Save abilityData;
    }

    private static SaveData SaveGameData()
    {
        if (!GlobalContainer.tryLoad<bool[]>("HomePhoto", out var homePhotoInit)) 
        {
            homePhotoInit = null;
        }

        if (!GlobalContainer.tryLoad<bool[]>("Ladder", out var ladderInit))
        {
            ladderInit = null;
        }

        return new SaveData
        {
            checkPoint = ControlManager.instance.startPoint,
            playerHeart = HeartManager.instance.heartNum,
            mapData = MapManager.SaveData(),
            questData = QuestManager.saveData(),
            inventoryData = Inventory.instance.SaveData(),
            abilityData = ControlManager.instance.player.GetComponent<PlayerAbilityTracker>().SaveData(),
            homePhotoInitialized = homePhotoInit,
            LadderInitialized = ladderInit
        };
    }

    private static bool LoadGameData(SaveData data)
    {
        if (data.mapData.index == MapManager.MapIndex.Login)
        {
            return false;
        }

        MapManager.LoadData(data.mapData);
        QuestManager.loadData(data.questData);
        GlobalContainer.store("inventory", data.inventoryData);
        GlobalContainer.store("StartPos", (Vector3)data.checkPoint);
        GlobalContainer.store("Heart", data.playerHeart);
        GlobalContainer.store("HomePhoto", data.homePhotoInitialized);
        GlobalContainer.store("Ladder", data.LadderInitialized);
        GlobalContainer.store("Ability", data.abilityData);

        return true;
    }

    public static void SaveGame()
    {
        var data = SaveGameData();

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

        if (!LoadGameData(data))
        {
            return null;
        }
        
        return data;
    }
}

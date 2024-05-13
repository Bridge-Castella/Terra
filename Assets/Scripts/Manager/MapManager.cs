using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager
{
    public enum MapIndex
    {
        Login = 0,
        Map1,
        Map2,
        Map3,
        Map4,
        Map5,
        Map6,
        Map7,
        Map8
    }

    public struct State
    {
        public MapIndex map;
        public MapIndex cleared;
        public MapIndex checkPoint;
        public MapStateChanger current;

        public static State CreateDefault()
        {
            return new State
            {
                map = MapIndex.Login,
                cleared = MapIndex.Login,
                checkPoint = MapIndex.Login
            };
        }
    }

    [System.Serializable]
    public struct Save
    {
        public MapIndex index;
        public MapIndex cleared;
        public bool[][] active;
    }

    public static State state;
    public static bool[][] active = new bool[8][];

    private static List<MapIndex> outOfRange = new List<MapIndex>();
    private static readonly int AdditiveMapStartIndex = 2;

    // load map asynchronously
    //  - When load has started successfully, it will return some AsyncOperation
    //  - When load is not needed or failed to load such index, it will return null
    public static AsyncOperation LoadMap(MapIndex index)
    {
        // load base scene(Map_0) on start(which is login scene)
        if (SceneManager.sceneCount == 1)
        {
            // load base scene first
            var load = SceneManager.LoadSceneAsync("02.Map_0");

            // load map
            // by loading map synchronously, it is ensured that game will start after the end of the loading
            SceneManager.LoadScene(ToSceneIndex(index), LoadSceneMode.Additive);

            // returning load(AsyncOperation) does not make sense
            // but this ensures that the map loading has been done properly
            return load;
        }

        int scene_index = ToSceneIndex(index);

        // if index is not a map or it has been already loaded, do nothing
        if (scene_index < AdditiveMapStartIndex || IsMapLoaded(index))
            return null;

        // load asynchronously
        return SceneManager.LoadSceneAsync(scene_index, LoadSceneMode.Additive);
    }

    // Unload map asynchronously
    //  - When unloading has started successfully, it will return some AsyncOperation
    //  - When unloading should not happen or failed to unload such index, it will return null
    public static AsyncOperation UnloadMap(MapIndex index)
    {
        if (SceneManager.sceneCount < 3)
        {
            return null;
        }

        int scene_index = ToSceneIndex(index);

        // if index is not a map or it is not loaded, do nothing
        if (scene_index < AdditiveMapStartIndex || !IsMapLoaded(index))
            return null;

        // if the map contains last checkpoint, dont unload it
        if (index == state.checkPoint)
        {
            // add it to outOfRange list
            // this will be checked later when the other checkpoint is reached
            outOfRange.Add(index);
            return null;
        }

        // unload asynchronously
        return SceneManager.UnloadSceneAsync(scene_index);
    }

    // Get map by index
    //  - the map must be loaded
    //  - if the map is not loaded, it will return null
    public static Scene? GetMap(MapIndex index)
    {
        int sceneIndex = ToSceneIndex(index);

        // search map for such index
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == sceneIndex)
                return scene;
        }
        return null;
    }

    // Find object by name
    //  - the map which is searched, must be loaded
    //  - this can only search root objects
    public static GameObject FindObjectInMap(MapIndex index, string name)
    {
        Scene? scene = GetMap(index);
        if (scene == null) return null;

        GameObject[] objects = ((Scene)scene).GetRootGameObjects();
        foreach (GameObject obj in objects)
        {
            if (obj.name == name) return obj;
        }

        return null;
    }

    // Check if the map is loaded
    public static bool IsMapLoaded(MapIndex index)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == ToSceneIndex(index))
                return true;
        }
        return false;
    }

    // Try remove map index from out of range
    //  - if such index exists, it will be removed and return true
    //  - if such index does not exist, it will return false
    public static bool TryRemoveFromOutOfRange(MapIndex index)
    {
        for (int i = 0; i < outOfRange.Count(); i++)
        {
            if (outOfRange[i] != index)
                continue;

            outOfRange.RemoveAt(i);
            return true;
        }

        return false;
    }

    // Convert scene index(Unity scene index) to map index(Terra map index)
    public static MapIndex ToMapIndex(int index)
    {
        return (MapIndex)(index - AdditiveMapStartIndex + 1);
    }

    // Convert map index(Terra map index) to scene index(Unity scene index)
    public static int ToSceneIndex(MapIndex index)
    {
        return (int)(index + AdditiveMapStartIndex - 1);
    }

    public static Save SaveData()
    {
        if (state.current == null)
            return new Save() { index = MapIndex.Login };

        // 현재 맵 상태 최신화
        state.current.SaveData();

        // Save 객체 생성
        var data = new Save
        {
            // CheckPoint가 기록된 맵 index 저장
            index = state.checkPoint,

            // 맵 active 정보 저장
            active = active
        };

        return data;
    }

    public static void LoadData(Save data)
    {
        state.checkPoint = data.index;
        active = data.active;
    }

    public static void ResetData()
    {
        state = State.CreateDefault();
        active = new bool[8][];
    }
}

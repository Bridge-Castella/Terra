using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MapManager;

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
        public MapIndex checkPoint;
        public MapStateChanger current;

        public static State createDefault()
        {
            State state = new State();
            state.map = MapIndex.Login;
            state.checkPoint = MapIndex.Login;
            return state;
        }
    }

    [System.Serializable]
    public struct Save
    {
        public MapIndex index;
        public bool[][] active;
    }

    public static State state;
    public static bool[][] active;

    private static List<MapIndex> outOfRange = new List<MapIndex>();
    private static int AdditiveMapStartIndex = 2;

    public static AsyncOperation LoadMap(MapIndex index)
    {
        if (SceneManager.sceneCount == 1)
        {
            var async = SceneManager.LoadSceneAsync("02.Map_0");
            SceneManager.LoadScene(ToSceneIndex(index), LoadSceneMode.Additive);
            return async;
        }

        int scene_index = ToSceneIndex(index);
        if (scene_index < AdditiveMapStartIndex || IsMapLoaded(index))
            return null;
        return SceneManager.LoadSceneAsync(scene_index, LoadSceneMode.Additive);
    }

    public static AsyncOperation UnloadMap(MapIndex index)
    {
        int scene_index = ToSceneIndex(index);
        if (scene_index < AdditiveMapStartIndex || !IsMapLoaded(index))
            return null;

        if (index == state.checkPoint)
        {
            outOfRange.Add(index);
            return null;
        }

        return SceneManager.UnloadSceneAsync(scene_index);
    }

    public static Scene? GetMap(MapIndex index)
    {
        int sceneIndex = ToSceneIndex(index);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == sceneIndex)
                return scene;
        }
        return null;
    }

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

    
    public static bool IsMapLoaded(MapIndex index)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == ToSceneIndex(index))
                return true;
        }
        return false;
    }

    public static bool IsMapOutOfRange(MapIndex index)
    {
        for (int i = 0; i < outOfRange.Count(); i++)
        {
            MapIndex map = outOfRange[i];
            if (map != index)
                continue;

            outOfRange.RemoveAt(i);
            return true;
        }

        return false;
    }

    public static MapIndex ToMapIndex(int index)
    {
        int mapIndex = index - AdditiveMapStartIndex + 1;
        return (MapIndex)mapIndex;
    }

    public static int ToSceneIndex(MapIndex index)
    {
        MapIndex sceneIndex = index + AdditiveMapStartIndex - 1;
        return (int)sceneIndex;
    }

    public static Save saveData()
    {
        // 현재 맵 상태 최신화
        state.current.saveData();

        // Save 객체 생성
        var data = new Save();

        // CheckPoint가 기록된 맵 index 저장
        data.index = state.checkPoint;

        // 맵 active 정보 저장
        data.active = active;

        return data;
    }

    public static void loadData(Save data)
    {
        state.checkPoint = data.index;
        active = data.active;
    }

    public static void resetData()
    {
        state = State.createDefault();
        active = new bool[8][];
    }
}

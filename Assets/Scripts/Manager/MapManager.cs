using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
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
    }

    public static State state = new State();
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
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    #region Singleton
    public static MapManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public enum MapState
    {
        Login = 0,
        Forest,
        Desert
    }

    public MapState mapState;


    [SerializeField] private int AdditiveMapStartIndex;

    public AsyncOperation LoadMap(int index)
    {
        if (SceneManager.sceneCount == 1)
        {
            var async = SceneManager.LoadSceneAsync("02.Map_0");
            SceneManager.LoadScene(ToSceneIndex(index), LoadSceneMode.Additive);
            return async;
        }

        int scene_index = ToSceneIndex(index);
        if (scene_index < AdditiveMapStartIndex || IsMapLoaded(scene_index)) return null;
        return SceneManager.LoadSceneAsync(scene_index, LoadSceneMode.Additive);
    }

    public AsyncOperation UnloadMap(int index)
    {
        int scene_index = ToSceneIndex(index);
        if (scene_index < AdditiveMapStartIndex || !IsMapLoaded(scene_index)) return null;
        return SceneManager.UnloadSceneAsync(scene_index);
    }

    public Scene? GetMap(int index)
    {
        int sceneIndex = ToSceneIndex(index);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex == sceneIndex) return scene;
        }
        return null;
    }

    public GameObject FindObjectInMap(int index, string name)
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

    private bool IsMapLoaded(int index)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == index) return true;
        }
        return false;
    }

    public int ToMapIndex(int index)
    {
        return index - AdditiveMapStartIndex + 1;
    }

    public int ToSceneIndex(int index)
    {
        return index + AdditiveMapStartIndex - 1;
    }
}

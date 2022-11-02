using System.Collections;
using System.Collections.Generic;
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
        Login,
        Forest,
        Dessert,
    }

    public MapState mapState;

    public void InventoryInit()
    {
        switch (mapState)
        {
            case MapState.Forest:
                ForestInventoryInit();
                break;
        }
    }

    public void ForestInventoryInit()
    {
        if (Inventory.instance != null)
            Inventory.instance.space = 1;
    }


    private static int additive_map_start_index = 3;

    public void LoadMap(string map = "")
    {
        if (map != "")
        {
            SceneManager.LoadSceneAsync(map);
            return;
        }

        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadSceneAsync("02.Map_0");
            SceneManager.LoadScene("Map_1", LoadSceneMode.Additive);
            return;
        }

        Scene? scene = GetCurrentMap();
        if (scene == null) return;
        int current = ((Scene)scene).buildIndex;
        SceneManager.LoadSceneAsync(++current, LoadSceneMode.Additive);
    }

    public Scene? GetCurrentMap()
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex >= additive_map_start_index) return scene;
        }
        return null;
    }

    public Scene? GetNextMap()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex < additive_map_start_index) continue;
            if (++i < SceneManager.sceneCount) return SceneManager.GetSceneAt(i);
        }
        return null;
    }
}

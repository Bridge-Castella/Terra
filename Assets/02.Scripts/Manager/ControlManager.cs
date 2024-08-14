using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    #region Singleton
    public static ControlManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    public Vector3 startPoint;
    public GameObject player;
    
    private void Start()
    {
        if (GameObject.Find("StartPoint") != null)
        {
            startPoint = GlobalContainer.tryLoadOrStore("StartPos",
            GameObject.Find("StartPoint").transform.position);
        }
    }

    public void RetryGame()
    {
        player.transform.SetParent(null);
        var primitiveMapIndex = MapManager.ToSceneIndex(MapManager.MapIndex.Login);
        var primitiveMap = SceneManager.GetSceneByBuildIndex(primitiveMapIndex);
        SceneManager.MoveGameObjectToScene(player, primitiveMap);
        
        player.transform.position = startPoint;
        HeartManager.instance.PlayerIsDead();
    }
}

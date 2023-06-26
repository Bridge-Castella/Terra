using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (GlobalContainer.contains("StartPos"))
            startPoint = GlobalContainer.load<Vector3>("StartPos");
        else
            startPoint = GameObject.Find("StartPoint").transform.position;
    }

    public void RetryGame()
    {
        player.transform.position = startPoint;
        HeartManager.instance.PlayerIsDead();
    }
}

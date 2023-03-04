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

    public Transform startPoint;
    public GameObject player;

    public GameObject optionObject;
    public GameObject optionObjectInstace;

    public bool gameIsPaused = false;

    [SerializeField] AK.Wwise.Event pause;
    [SerializeField] AK.Wwise.Event resume;

    private void Start()
    {
        startPoint = GameObject.Find("StartPoint").transform;
    }

    private void Update()
    {
        Option();
    }

    public void Option()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //AudioManager.instance.PlaySound("ui_02");                             // Outdated audio engine
        resume.Post(gameObject);
        //Destroy(optionObjectInstace);
        optionObject.GetComponent<Option>().OnClickCancelButton();
        optionObject.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        //AudioManager.instance.PlaySound("ui_01");                             // Outdated audio engine
        pause.Post(gameObject);
        //optionObjectInstace = Instantiate(optionObject, FindObjectOfType<Canvas>().gameObject.transform);
        optionObject.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void RetryGame()
    {
        player.transform.position = startPoint.position;
        HeartManager.instance.PlayerIsDead();
    }
}

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

    public GameObject optionObject;
    public GameObject optionObjectInstace;

    public bool gameIsPaused = false;

    [SerializeField] AK.Wwise.Event pause;
    [SerializeField] AK.Wwise.Event resume;

    private void Start()
    {
        if (GlobalContainer.contains("StartPos"))
            startPoint = GlobalContainer.load<Vector3>("StartPos");
        else
            startPoint = GameObject.Find("StartPoint").transform.position;
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
        if (resume != null)
            resume.Post(gameObject);

        //Destroy(optionObjectInstace);
        optionObject.GetComponent<Option>().OnClickCancelButton();
        optionObject.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        if (pause != null)
            pause.Post(gameObject);

        //optionObjectInstace = Instantiate(optionObject, FindObjectOfType<Canvas>().gameObject.transform);
        optionObject.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void RetryGame()
    {
        player.transform.position = startPoint;
        HeartManager.instance.PlayerIsDead();
    }
}

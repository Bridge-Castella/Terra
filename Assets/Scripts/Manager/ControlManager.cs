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

    public GameObject startPoint;
    public GameObject player;

    public GameObject optionObject;
    public GameObject optionObjectInstace;

    public bool gameIsPaused = false;


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
        AudioManager.instance.PlaySound("ui_02");
        Destroy(optionObjectInstace);
        //OptionObject.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        AudioManager.instance.PlaySound("ui_01");
        optionObjectInstace = Instantiate(optionObject, FindObjectOfType<Canvas>().gameObject.transform);
        //OptionObject.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void RetryGame()
    {
        player.transform.position = startPoint.transform.position;
        int fullHealAmount = HeartHealthSystem.MAX_FRAGMENT_AMOUNT * HeartsHealthVisual.heartHealthSystemStatic.GetHeartList().Count;
        HeartsHealthVisual.heartHealthSystemStatic.Heal(fullHealAmount);
    }
}

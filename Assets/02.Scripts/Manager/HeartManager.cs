using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartManager : MonoBehaviour
{
    #region Singleton
    public static HeartManager instance;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    public bool IsRespawning { get; private set; }

    public int heartNum = 5;
    public TextMeshProUGUI heartNumText;

    void Start()
    {
        if (GlobalContainer.contains("Heart"))
            heartNum = GlobalContainer.load<int>("Heart");

        heartNumText.text = heartNum.ToString();
    }

    public void GetDamage(int damage = 1)
    {
        if (heartNum <= 0)
        {
            PlayerIsDead();
            return;
        }

        heartNum -= damage;
        heartNumText.text = heartNum.ToString();
        PlayerAudio.Post(PlayerAudio.Instance.inGame_CH_Life);
    }

    public void PlayerIsDead()
    {
        IsRespawning = true;
        heartNum = 5;
        heartNumText.text = heartNum.ToString();
        PlayerAudio.Post(PlayerAudio.Instance.inGame_CH_Die);

        StartCoroutine(CoWaitForRespawn());
    }

    public bool IsPlayerDead()
    {
        if(heartNum <= 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator CoWaitForRespawn()
    {
        yield return new WaitForSeconds(0.1f);

        IsRespawning = false;
    }
}

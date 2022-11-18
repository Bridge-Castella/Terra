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

    public int heartNum = 5;
    public TextMeshProUGUI heartNumText;

    void Start()
    {
        heartNumText.text = heartNum.ToString();
    }

    public void GetDamage()
    {
        if(heartNum <= 0)
            PlayerIsDead();
        heartNum --;
        heartNumText.text = heartNum.ToString();
    }

    public void PlayerIsDead()
    {
        heartNum = 5;
        heartNumText.text = heartNum.ToString();
    }
}

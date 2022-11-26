using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private int damageAmount;

    Transform startPoint; //죽었을 경우 체크포인트 start
    PlayerMove player;

    [SerializeField] AK.Wwise.Event life;
    [SerializeField] GameObject soundObject;

    private void Start()
    {
        startPoint = checkPoint;
    }

    public Transform CheckPoint
    {
        get { return checkPoint;}
        set { checkPoint = value;}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Player")
            return;

        StartCoroutine(CoPlayerStop());

        //if(AudioManager.instance != null)                                     // Outdated audio engine
        //AudioManager.instance.PlaySound("life_01");
        life.Post(soundObject);

        player = collision.gameObject.GetComponent<PlayerMove>();
        player.isFalling = true;

        if(!player.isHurting)
        {
            player.DamageFlash();
            HeartManager.instance.GetDamage();
            StartCoroutine(player.CoEnableDamage(0.5f, 1.5f));
        }

        if (HeartsHealthVisual.heartHealthSystemStatic.IsDead())             
        {
            ControlManager.instance.RetryGame();
            checkPoint = startPoint; //체크포인트 다시 startpoint로 
            return;
        }
    }

    //플레이어 떨어지면 일시정지
    IEnumerator CoPlayerStop()
    {
        yield return new WaitForSeconds(0.5f);
        player.isFalling = false;
        player.gameObject.transform.position = checkPoint.position;
    }
}

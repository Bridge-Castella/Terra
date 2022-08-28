using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [SerializeField] private Transform playerObject;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private GameObject[] fadingPlatforms;
    [SerializeField] private int damageAmount;

    Transform startPoint; //죽었을 경우 체크포인트 start

    private void Start()
    {
        fadingPlatforms = GameObject.FindGameObjectsWithTag("FadingPlatform");
        startPoint = checkPoint;
    }

    public Transform CheckPoint
    {
        get { return checkPoint;}
        set { checkPoint = value;}
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {   
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound("life_01");
        PlayerMove player = collider.gameObject.GetComponent<PlayerMove>();
        if(!player.isHurting)
        {
            player.DamageFlash();
            HeartsHealthVisual.heartHealthSystemStatic.Damage(damageAmount);
            StartCoroutine(player.CoEnableDamage(0.5f, 1.5f));
        }

        for (int i = 0; i < fadingPlatforms.Length; i++)
        {
            FadingPlatform fadingPlatformItem = fadingPlatforms[i].GetComponent<FadingPlatform>();
            fadingPlatformItem.ShowFadingPlatform();
        }

        if (HeartsHealthVisual.heartHealthSystemStatic.IsDead())
        {
            ControlManager.instance.RetryGame();
            checkPoint = startPoint; //체크포인트 다시 startpoint로 
            return;
        }
        playerObject.position = checkPoint.position;    
    }
}

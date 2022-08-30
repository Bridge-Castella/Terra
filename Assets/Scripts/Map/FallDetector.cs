using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private GameObject[] fadingPlatforms;
    [SerializeField] private int damageAmount;

    Transform startPoint; //�׾��� ��� üũ����Ʈ start
    PlayerMove player;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Player")
            return;

        StartCoroutine(CoPlayerStop());
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound("life_01");

        player = collision.gameObject.GetComponent<PlayerMove>();
        player.isFalling = true;

        if(!player.isHurting)
        {
            player.DamageFlash();
            HeartsHealthVisual.heartHealthSystemStatic.Damage(damageAmount);
            StartCoroutine(player.CoEnableDamage(0.5f, 1.5f));
        }

        if (HeartsHealthVisual.heartHealthSystemStatic.IsDead())
        {
            //TODO ��� ���Ƴ���
            //ControlManager.instance.RetryGame();
            //checkPoint = startPoint; //üũ����Ʈ �ٽ� startpoint�� 
            return;
        }
    }

    IEnumerator CoPlayerStop()
    {
        yield return new WaitForSeconds(0.5f);
        player.isFalling = false;
        player.gameObject.transform.position = checkPoint.position;

        for (int i = 0; i < fadingPlatforms.Length; i++)
        {
            FadingPlatform fadingPlatformItem = fadingPlatforms[i].GetComponent<FadingPlatform>();
            fadingPlatformItem.ShowFadingPlatform();
        }
    }
}

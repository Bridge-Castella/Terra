using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
        if(player != null)
        {
            if(!player.isHurting)
            {
                player.DamageKnockBack(gameObject.transform.position,1);
            }
            if(HeartManager.instance.IsPlayerDead())
            {
                ControlManager.instance.RetryGame();
            }
        }
    }
}

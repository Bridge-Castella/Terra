using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float knockBackPower = 30f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
        if(player != null)
        {
            if(!player.isHurting)
            {
                player.DamageKnockBack(gameObject.transform.position, knockBackPower);
            }
            if(HeartManager.instance.IsPlayerDead())
            {
                ControlManager.instance.RetryGame();
            }
        }
    }
}

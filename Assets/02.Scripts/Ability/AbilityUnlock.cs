using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    public bool unlockDoubleJump, unlockDash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerAbilityTracker playerAT = collision.GetComponent<PlayerAbilityTracker>();
            if(unlockDoubleJump)
            {
                playerAT.canDoubleJump = true;
            }

            if (unlockDash)
            {
                playerAT.canDash = true;
            }

            Destroy(gameObject);
        }
    }
}

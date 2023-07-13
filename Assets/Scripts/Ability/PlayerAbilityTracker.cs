using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTracker : MonoBehaviour
{
    public bool canDoubleJump, canDash, canClimb;

    [HideInInspector] public bool isFlying, isSpringJump, isHoldingFire;

    [HideInInspector] public float springJumpPower;
}

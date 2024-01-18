using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTracker : MonoBehaviour
{
    public bool canDoubleJump, canDash, canClimb;

    [HideInInspector] public bool isFlying, isSpringJump, isHoldingFire;
    public GameObject wing;

    [HideInInspector] public float springJumpPower;
}

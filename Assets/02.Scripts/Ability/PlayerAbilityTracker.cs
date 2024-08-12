using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTracker : MonoBehaviour
{
    public bool canDoubleJump;
    public bool canDash;
    public bool canClimb;

    [HideInInspector] public bool isFlying, isSpringJump, isHoldingFire;
    public GameObject wing;

    [HideInInspector] public float springJumpPower;

    private void Awake()
    {
        if (GlobalContainer.contains("Ability"))
        {
            LoadData(GlobalContainer.load<Save>("Ability"));
        }
    }

    [System.Serializable]
    public struct Save
    {
        public bool canDoubleJump;
        public bool canDash;
        public bool canClimb;
    }

    public Save SaveData()
    {
        return new Save()
        {
            canDoubleJump = this.canDoubleJump,
            canDash = this.canDash,
            canClimb = this.canClimb,
        };
    }

    public void LoadData(Save data)
    {
        canDoubleJump = data.canDoubleJump;
        canDash = data.canDash;
        canClimb = data.canClimb;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class BonfireInteraction : Interaction
{
    [SerializeField] private Light2D light;

    public override void InteractionWithBonfire()
    {
        if(!abilities.isHoldingFire)
            return;
        light.intensity = 1f;
        abilities.isHoldingFire = false;
        FireItem fire = (FireItem)Inventory.instance.SelectItem(300);
        fire.UseFireItem();
    }
}

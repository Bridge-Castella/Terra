using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class BonfireInteraction : Interaction
{
    [SerializeField] private GameObject lightObject;

    public override void InteractionWithBonfire()
    {
        if(!abilities.isHoldingFire)
            return;
        lightObject.GetComponent<Light2D>().intensity = 1f;
        abilities.isHoldingFire = false;
    }
}

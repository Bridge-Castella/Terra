using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class BonfireInteraction : Interaction
{
    [SerializeField] private Light2D light;

    public override void InteractionWithBonfire()
    {
        if(!abilities.isHoldingFire)
            return;
        light.gameObject.transform.DOScale(1f, 1f);
        StartCoroutine(CoIncreaseIntensity());
        abilities.isHoldingFire = false;
        FireItem fire = (FireItem)Inventory.instance.SelectItem(300);
        fire.UseFireItem();
    }

    private IEnumerator CoIncreaseIntensity()
    {
        while (light.intensity < 1f)
        {
            light.intensity += Time.deltaTime * 1f;
            yield return null;
        }
    }
}

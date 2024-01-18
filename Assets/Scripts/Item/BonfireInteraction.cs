using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class BonfireInteraction : Interaction
{
    [SerializeField] private Light2D light;
    [SerializeField] private Sprite fireOnSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public override void InteractionWithBonfire()
    {
        if(!abilities.isHoldingFire)
            return;
        spriteRenderer.sprite = fireOnSprite;
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

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class BonfireInteraction : Interaction
{
    [SerializeField] private Light2D light;
    [SerializeField] private Sprite fireOnSprite;
    [SerializeField] private CanvasGroup btnCanvas;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float soundMinThreshold = 15f;
    public float soundMaxThreshold = 10f;

    protected override void Update()
    {
        base.Update();

        if (isInCamera())
        {
            var cameraPos = Camera.main.transform.position;
            var distance = Vector2.Distance(cameraPos, transform.position);
            
            if (distance < soundMaxThreshold)
            {
                AudioManager.instance.SetFireVolume(1.0f);
            }
            else
            {
                float diff = distance - soundMaxThreshold;
                float volume = 1 - (diff / (soundMinThreshold - soundMaxThreshold));
                AudioManager.instance.SetFireVolume(volume);
            }
        }
    }

    public override void InteractionWithBonfire()
    {
        if(!abilities.isHoldingFire)
            return;

        InGameAudio.Post(InGameAudio.Instance.ITEM_Fire_03);
        InGameAudio.Post(InGameAudio.Instance.ITEM_Fire_01);

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

    private bool isInCamera()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            return true;
        }

        return false;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(abilities.isHoldingFire)
            btnCanvas.DOFade(1f, 0.3f);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (abilities.isHoldingFire)
            btnCanvas.DOFade(0f, 0.3f);
    }
}

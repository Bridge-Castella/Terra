using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WingItem : Item
{
    [SerializeField] private float wingTime = 3f;
    [SerializeField] private float showTime = 2f;
    [SerializeField] GameObject itemSprite;
    [SerializeField] GameObject timeIndicator;

    private PlayerMove player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
    }

    public override void GetWingItem()
    {
        abilities.isFlying = true;
        itemSprite.SetActive(false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(CoUseWingItem());
    }

    IEnumerator CoUseWingItem()
    {
        // TODO: 사운드 재생시점 변경
        InGameAudio.Post(InGameAudio.Instance.ITEM_Wing_01);
        InGameAudio.Post(InGameAudio.Instance.ITEM_Wing_02);

        var indicator = Instantiate(timeIndicator, player.transform);
        var indicatorText = indicator.GetComponentInChildren<TextMeshProUGUI>();
        var timeElasped = 0f;

        while (timeElasped < wingTime)
        {
            timeElasped += Time.deltaTime;

            var remaining = wingTime - timeElasped;
            if (remaining < 0)
            {
                remaining = 0f;
            }

            if (remaining > 1.0f)
            {
                indicatorText.text = Mathf.CeilToInt(remaining).ToString();
            }
            else
            {
                indicatorText.text = remaining.ToString("F1");
            }

            var scale = indicator.transform.localScale;
            scale.x = player.facingRight ? Mathf.Abs(scale.x) : Mathf.Abs(scale.x) * -1;
            indicator.transform.localScale = scale;

            yield return null;
        }

        abilities.isFlying = false;
        indicator.gameObject.SetActive(false);
        Destroy(indicator);
        StartCoroutine(CoShowWingItem());
    }

    IEnumerator CoShowWingItem()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        itemSprite.SetActive(true);
    }
}

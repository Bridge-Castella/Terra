using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    public float UpdateInterval = 0.016f;
    public float FadeInterval = 0.01f;
    public float FadeInDelay = 0.5f;
    public float FadeOutDelay = 0.0f;

    private SpriteRenderer[] background = null;

    private void Start()
    {
        initBackground();
        SetAlpha(0.0f);
    }

    private void OnEnable()
    {
        StartFadeIn();
    }

    public void SetAlpha(float alpha)
    {
        initBackground();
        if (background == null) return;
        foreach (SpriteRenderer bg in background)
        {
            Color color = bg.color;
            color.a = alpha;
            bg.color = color;
        }
    }

    public void StartFadeIn()
    {
        initBackground();
        if (background == null) return;
        StopAllCoroutines();
        foreach (SpriteRenderer bg in background)
        {
            StartCoroutine(FadeIn(bg, FadeInDelay));
        }
    }

    public void StartFadeOut()
    {
        initBackground();
        if (background == null) return;
        StopAllCoroutines();
        foreach (SpriteRenderer bg in background)
        {
            StartCoroutine(FadeOut(bg, FadeOutDelay));
        }
    }

    private IEnumerator FadeIn(SpriteRenderer renderer, float delay)
    {
        float alpha = renderer.color.a;
        Color color = renderer.color;

        yield return new WaitForSeconds(delay);
        while (renderer.color.a < 1.0f)
        {
            alpha += FadeInterval;
            color.a = alpha;
            renderer.color = color;
            yield return new WaitForSeconds(UpdateInterval);
        }
    }

    private IEnumerator FadeOut(SpriteRenderer renderer, float delay)
    {
        float alpha = renderer.color.a;
        Color color = renderer.color;

        yield return new WaitForSeconds(delay);
        while (renderer.color.a > 0)
        {
            alpha -= FadeInterval;
            color.a = alpha;
            renderer.color = color;
            yield return new WaitForSeconds(UpdateInterval);
        }
    }

    public float GetFadeSeconds(float endAlpha)
    {
        float alpha_diff = Math.Abs(GetAlpha() - endAlpha);
        return (alpha_diff / FadeInterval) * UpdateInterval * 1.5f;
    }

    public float GetAlpha()
    {
        initBackground();
        if (background == null) return 0.0f;
        return background[0].color.a;
    }

    public void initBackground()
    {
        if (background != null)
        {
            if (background.Length > 0) return;
        }
        background = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }
}
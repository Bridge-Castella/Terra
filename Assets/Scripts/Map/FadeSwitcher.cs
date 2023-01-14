using System;
using System.Collections;
using UnityEngine;

public class FadeSwitcher : MonoBehaviour
{
    public float UpdateInterval = 0.016f;
    public float FadeInterval = 0.01f;
    public float FadeInDelay = 0.5f;
    public float FadeOutDelay = 0.0f;

    private SpriteRenderer[] sprite = null;

	public void SetAlpha(float alpha)
    {
        foreach (SpriteRenderer bg in sprite)
        {
            Color color = bg.color;
            color.a = alpha;
            bg.color = color;
        }
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
		foreach (SpriteRenderer bg in sprite)
        {
            StartCoroutine(FadeIn(bg, FadeInDelay));
        }
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        foreach (SpriteRenderer bg in sprite)
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
        while (renderer.color.a > 0.0)
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
        return sprite[0].color.a;
    }

    public void InitSprite(GameObject environment)
    {
        if (sprite != null)
        {
            if (sprite.Length > 0) return;
        }
        sprite = environment.GetComponentsInChildren<SpriteRenderer>();
    }
}
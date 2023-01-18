using System;
using System.Collections;
using UnityEngine;

public class FadeSwitcher : MonoBehaviour
{
    public FadeSettings settings = new FadeSettings();
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

    public void StartFadeIn(FadeSettings setting = null)
    {
        StopAllCoroutines();
		foreach (SpriteRenderer bg in sprite)
        {
            StartCoroutine(FadeIn(bg, setting));
        }
    }

    public void StartFadeOut(FadeSettings setting = null)
    {
		StopAllCoroutines();
        foreach (SpriteRenderer bg in sprite)
        {
            StartCoroutine(FadeOut(bg, setting));
        }
    }

    private IEnumerator FadeIn(SpriteRenderer renderer, FadeSettings custom)
    {
        float alpha = renderer.color.a;
        Color color = renderer.color;
        FadeSettings.FadeValues setting = custom == null ? settings.fadeIn : custom.fadeIn;
        if (setting.initAlpha >= 0.0f)
        {
            color.a = setting.initAlpha;
            renderer.color = color;
        }

        yield return new WaitForSeconds(setting.startDelay);
        while (renderer.color.a < setting.targetAlpha)
        {
            alpha += setting.alphaInterval;
            color.a = alpha;
            renderer.color = color;
            yield return new WaitForSeconds(setting.updateInterval);
        }
    }

    private IEnumerator FadeOut(SpriteRenderer renderer, FadeSettings custom)
    {
        float alpha = renderer.color.a;
        Color color = renderer.color;
		FadeSettings.FadeValues setting = custom == null ? settings.fadeOut : custom.fadeOut;
		if (setting.initAlpha >= 0.0f)
		{
			color.a = setting.initAlpha;
			renderer.color = color;
		}

		yield return new WaitForSeconds(setting.startDelay);
        while (renderer.color.a > setting.targetAlpha)
        {
            alpha -= setting.alphaInterval;
            color.a = alpha;
            renderer.color = color;
            yield return new WaitForSeconds(setting.updateInterval);
        }
    }

    public float GetFadeInSeconds()
    {
        float alpha_diff = settings.fadeIn.targetAlpha - GetAlpha();
        if (alpha_diff < 0.0f) return 0.0f;
        return (alpha_diff / settings.fadeIn.alphaInterval) * settings.fadeIn.updateInterval * 1.5f;
    }

    public float GetFadeOutSeconds()
    {
        float alpha_diff = GetAlpha() - settings.fadeOut.targetAlpha;
        if (alpha_diff < 0.0f) return 0.0f;
        return (alpha_diff / settings.fadeOut.alphaInterval) * settings.fadeOut.updateInterval * 1.5f;
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
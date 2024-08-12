using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
	#region Singleton
	public static SceneFader instance;

	private void Awake()
	{
		if (instance != null)
			Destroy(gameObject);
		else
		{
			instance = this;
		}
	}
	#endregion

	public bool Active { get; private set; }
	public bool enableFade;
	public float fadeDuration;
	public float beforeDelay;
	public float afterDelay;
	[SerializeField] Image image;

	public delegate void CallbackFunc();
    public CallbackFunc OnFadeStart;
    public CallbackFunc OnFadeEnd;
	public CallbackFunc OnProcessEnd;

	public void FadeIn(float duration = -1f)
	{
		StopAllCoroutines();
		StartCoroutine(_Fade(1f, duration < 0f ? fadeDuration : duration));
	}

	public void FadeOut(float duration = -1f)
	{
        StopAllCoroutines();
        StartCoroutine(_Fade(0f, duration < 0f ? fadeDuration : duration));
    }

    private IEnumerator _Fade(float targetAlpha, float duration)
	{
		Active = true;
		float originalAlpha = image.color.a;
        float timeElapsed = 0f;

        while (timeElapsed < beforeDelay)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

		OnFadeStart?.Invoke();
		OnFadeStart = null;

		timeElapsed = 0f;
        while (timeElapsed < duration)
        {
			if (enableFade)
			{
				var color = image.color;
				color.a = Mathf.Lerp(originalAlpha, targetAlpha, timeElapsed / duration);
				image.color = color;
			}
            timeElapsed += Time.deltaTime;
            yield return null;
        }

		if (enableFade)
		{
			var color = image.color;
			color.a = targetAlpha;
			image.color = color;
		}

        OnFadeEnd?.Invoke();
        OnFadeEnd = null;

        timeElapsed = 0f;
		while (timeElapsed < afterDelay)
		{
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		OnProcessEnd?.Invoke();
		OnProcessEnd = null;
		Active = false;
    }
}

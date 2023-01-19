using System;

[Serializable]
public class FadeSettings
{
	[Serializable]
	public struct FadeValues
	{
		public float updateInterval;
		public float startDelay;
		public float alphaInterval;
		public float targetAlpha;
		public float initAlpha;
	}

	public bool dontFade = false;
	public FadeValues fadeIn = DefaultFadeIn();
	public FadeValues fadeOut = DefaultFadeOut();
	
	public static FadeValues DefaultFadeIn()
	{
		FadeValues setting = new FadeValues();
		setting.updateInterval = 0.016f;
		setting.startDelay = 0.0f;
		setting.alphaInterval = 0.01f;
		setting.targetAlpha = 1.0f;
		setting.initAlpha = -1.0f;
		return setting;
	}

	public static FadeValues DefaultFadeOut()
	{
		FadeValues setting = new FadeValues();
		setting.updateInterval = 0.016f;
		setting.startDelay = 0.0f;
		setting.alphaInterval = 0.01f;
		setting.targetAlpha = 0.0f;
		setting.initAlpha = -1.0f;
		return setting;
	}
}
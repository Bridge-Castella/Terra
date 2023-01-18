using System;

[Serializable]
public class FadeSettings
{
	[Serializable]
	public class FadeValues
	{
		public float updateInterval = 0.016f;
		public float startDelay = 0.0f;
		public float alphaInterval = 0.01f;
		public float targetAlpha = 0.0f;
		public float initAlpha = 0.0f;
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
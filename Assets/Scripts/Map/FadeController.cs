using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
	public bool initializePosition = true;

	public GameObject[] environment;
    [HideInInspector] public CustomFadeArea customArea = null;

    private FadeSwitcher switcher;
    private static bool firstScene = true;
    private bool firstLoad = true;

    private void Start()
    {
        switcher = gameObject.GetComponent<FadeSwitcher>();
    }

    public static void resetFade()
    {
        firstScene = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            bool dontFade = false;
            FadeSettings customSetting = null;
            if (customArea != null)
            {
                if (customArea.settings.dontFade)
                    dontFade = true;
                customSetting = customArea.settings;
            }
            
            foreach (GameObject env in environment)
            {
                if (!env.activeSelf) env.SetActive(true);
                if (firstLoad)
                {
					firstLoad = false;
					switcher.InitSprite(env);

					if (initializePosition && !firstScene)
						InitBackgroundPosition(env);

                    if (dontFade)
                    {
                        switcher.SetAlpha(1.0f);
                        continue;
                    }

                    switcher.SetAlpha(0.0f);
                    if (firstScene)
                    {
                        switcher.SetAlpha(0.6f);
                        firstScene = false;
                    }                    
				}
                switcher.StartFadeIn(customSetting);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            FadeSettings customSetting = null;
            if (customArea != null)
            {
                if (customArea.settings.dontFade)
                    return;
                customSetting = customArea.settings;
            }

            switcher.StartFadeOut(customSetting);
            StartCoroutine(WaitForBackgroundToFinish());
        }
    }

	private void InitBackgroundPosition(GameObject env)
	{
		foreach (ScrollingBackground ele in env.GetComponentsInChildren<ScrollingBackground>())
		{
			ele.InitPosition();
		}
	}

    private IEnumerator WaitForBackgroundToFinish()
    {
        yield return new WaitForSeconds(switcher.GetFadeOutSeconds());
        if (switcher.GetAlpha() == 0.0f)
        {
            foreach (GameObject ele in environment)
            {
                ele.SetActive(false);
            }
        }
    }
}

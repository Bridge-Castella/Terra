using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public GameObject[] environment;
    public GameObject[] nonFadeArea = new GameObject[] { };

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
            bool donotFade = false;
            foreach (GameObject ele in nonFadeArea)
            {
                if (ele.GetComponent<AreaTriggerCallback>().isTriggered)
                {
					donotFade = true;
                    break;
                }
            }

            foreach (GameObject ele in environment)
            {
                if (!ele.activeSelf) ele.SetActive(true);
                if (firstLoad)
                {
					firstLoad = false;
					switcher.InitSprite(ele);

                    if (donotFade)
                    {
                        switcher.SetAlpha(1.0f);
                        InitBackgroundPosition(ele);
                        continue;
                    }

                    switcher.SetAlpha(0.0f);
					if (firstScene)
					{
						switcher.SetAlpha(0.6f);
						firstScene = false;
					}
                    else InitBackgroundPosition(ele);
				}
                switcher.StartFadeIn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            foreach (GameObject ele in nonFadeArea)
            {
                if (ele.GetComponent<AreaTriggerCallback>().isTriggered)
                {
                    switcher.SetAlpha(0.0f);
                    return;
                }
            }

            switcher.StartFadeOut();
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
        yield return new WaitForSeconds(switcher.GetFadeSeconds(0.0f));
        if (switcher.GetAlpha() == 0.0f)
        {
            foreach (GameObject ele in environment)
            {
                ele.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public GameObject[] environment;
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
            foreach (GameObject ele in environment)
            {
                if (!ele.activeSelf) ele.SetActive(true);
                if (firstLoad)
                {
                    switcher.SetAlpha(0.0f);
                    firstLoad = false;
					if (firstScene)
					{
						switcher.SetAlpha(0.6f);
						firstScene = false;
					}
				}
                switcher.StartFadeIn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            switcher.StartFadeOut();
            StartCoroutine(WaitForBackgroundToFinish());
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

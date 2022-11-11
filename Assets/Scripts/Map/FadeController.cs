using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public GameObject[] environment;
    private FadeSwitcher switcher;

    private void Start()
    {
        switcher = gameObject.GetComponent<FadeSwitcher>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            foreach (GameObject ele in environment)
            {
                if (!ele.activeSelf) ele.SetActive(true);
                else switcher.StartFadeIn();
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

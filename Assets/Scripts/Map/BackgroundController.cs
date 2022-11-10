using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject background;
    private BackgroundSwitcher switcher;

    private void Start()
    {
        switcher = gameObject.GetComponent<BackgroundSwitcher>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (!background.activeSelf) background.SetActive(true);
            else switcher.StartFadeIn();
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
        if (switcher.GetAlpha() == 0.0f) background.SetActive(false);
    }
}

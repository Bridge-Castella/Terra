using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingGroundEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] scrollingGrounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        StartCoroutine(SetActive(true));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        StartCoroutine(SetActive(false));
    }

    private IEnumerator SetActive(bool active)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var sg in scrollingGrounds)
        {
            sg.SetActive(active);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] scrollingGrounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Map state 변경
        int sceneIndex = gameObject.scene.buildIndex;
        MapManager.MapIndex mapIndex = MapManager.ToMapIndex(sceneIndex);
        MapManager.state.map = mapIndex;

        // Scrolling Ground 활성화
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

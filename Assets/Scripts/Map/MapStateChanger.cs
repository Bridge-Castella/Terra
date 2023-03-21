using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateChanger : MonoBehaviour
{
    [Header("움직이는 배경 Groups")]
    [SerializeField] private GameObject[] scrollingGrounds;

    [Header("활성화 여부를 체크해야 하는 Object Groups")]
    [SerializeField] private GameObject[] objectGroups;

    private MapManager.MapIndex mapIndex;

    private void Start()
    {
        // Login Scene 제외 map index
        int sceneIndex = gameObject.scene.buildIndex;
        mapIndex = MapManager.ToMapIndex(sceneIndex);

        // Load Data from saved file
        loadData();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Map state 변경
        MapManager.state.map = mapIndex;
        MapManager.state.current = this;

        // Scrolling Ground 활성화
        StartCoroutine(SetActive(true));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Disable scrolling background
        StartCoroutine(SetActive(false));

        // Save active state objects before unload
        saveData();
    }

    private IEnumerator SetActive(bool active)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var sg in scrollingGrounds)
        {
            sg.SetActive(active);
        }
    }

    public void loadData()
    {
        int intMapIndex = (int)mapIndex - 1;
        bool[] active = MapManager.active[intMapIndex];
        if (active == null)
            return;

        int counter = 0;
        foreach (var objectGroup in objectGroups)
        {
            for (int i = 0; i < objectGroup.transform.childCount; i++)
                objectGroup.transform.GetChild(i).gameObject.SetActive(active[counter++]);
        }
    }

    public void saveData()
    {
        List<bool> active = new List<bool>();
        foreach (var objectGroup in objectGroups)
        {
            for (int i = 0; i < objectGroup.transform.childCount; i++)
                active.Add(objectGroup.transform.GetChild(i).gameObject.activeSelf);
        }

        int intMapIndex = (int)mapIndex - 1;
        MapManager.active[intMapIndex] = active.ToArray();
    }
}

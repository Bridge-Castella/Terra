using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateChanger : MonoBehaviour
{
    public static AK.Wwise.Event CurrentMapBGM;

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
        LoadData();
    }

    private void OnDestroy()
    {
        InGameAudio.Stop(InGameAudio.Instance.BGM_MAP1_loop);
        InGameAudio.Stop(InGameAudio.Instance.BGM_MAP2_loop);
        InGameAudio.Stop(InGameAudio.Instance.BGM_MAP3_loop);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Map state 변경
        MapManager.state.map = mapIndex;
        MapManager.state.current = this;

        CurrentMapBGM = mapIndex switch
        {
            MapManager.MapIndex.Map1 => InGameAudio.Instance.BGM_MAP1_loop,
            MapManager.MapIndex.Map2 => InGameAudio.Instance.BGM_MAP2_loop,
            MapManager.MapIndex.Map3 => InGameAudio.Instance.BGM_MAP3_loop,
            _ => null,
        };

        InGameAudio.Post(CurrentMapBGM);

        // Scrolling Ground 활성화
        SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Disable scrolling background
        SetActive(false);

        // 아직 클리어 하지 않았다면 Clear 사운드 재생
        if (MapManager.state.cleared < mapIndex)
        {
            MapManager.state.cleared = mapIndex;

            if (!HeartManager.instance.IsRespawning)
            {
                InGameAudio.Post(InGameAudio.Instance.inGame_STAGE_CLEAR);
            }
        }

        var bgmToStop = mapIndex switch
        {
            MapManager.MapIndex.Map1 => InGameAudio.Instance.BGM_MAP1_loop,
            MapManager.MapIndex.Map2 => InGameAudio.Instance.BGM_MAP2_loop,
            MapManager.MapIndex.Map3 => InGameAudio.Instance.BGM_MAP3_loop,
            _ => null,
        };
        
        InGameAudio.Stop(bgmToStop);

        // Save active state objects before unload
        SaveData();
    }

    private void SetActive(bool active)
    {
        //yield return new WaitForSeconds(0.1f);
        foreach (var sg in scrollingGrounds)
        {
            sg.SetActive(active);
        }
    }

    public void LoadData()
    {
        int intMapIndex = (int)mapIndex - 1;
        bool[] active = MapManager.active[intMapIndex];
        if (active == null || objectGroups == null)
            return;

        int counter = 0;
        foreach (var objectGroup in objectGroups)
        {
            for (int i = 0; i < objectGroup.transform.childCount; i++)
                objectGroup.transform.GetChild(i).gameObject.SetActive(active[counter++]);
        }
    }

    public void SaveData()
    {
        if (objectGroups == null)
            return;

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

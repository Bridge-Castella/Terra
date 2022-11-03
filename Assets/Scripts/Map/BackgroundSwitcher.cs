using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSwitcher : MonoBehaviour
{
    private enum FADE
    {
        IN = 0,
        OUT = 1
    };

    public float update_interval = 0.016f;
    public float fade_interval = 0.01f;
    public float overlap_amount = 0.5f;

    private bool is_activated = false;
   
    private List<Coroutine> activated_corutine = new List<Coroutine>();
    private List<SpriteRenderer>[] background = new List<SpriteRenderer>[2];
    private List<GameObject>[] background_object = new List<GameObject>[2];
    private List<int>[] background_counter = new List<int>[2];

    private GameObject player_ptr;
    private int[] fade_index = new int[2];
    private int[] map_build_index = new int[2];
    private GameObject[][] object_array = new GameObject[2][];
    private SpriteRenderer[][] background_array = new SpriteRenderer[2][];

    private static Dictionary<int, List<Vector3>> background_position = new Dictionary<int, List<Vector3>>();

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            background[i] = new List<SpriteRenderer>();
            background_object[i] = new List<GameObject>();
            background_counter[i] = new List<int>();
        }
        player_ptr = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (is_activated)
        {
            return;
            //foreach (Coroutine coroutine in activated_corutine)
            //    StopCoroutine(coroutine);
        }

        Scene? map_1 = MapManager.instance.GetCurrentMap();
        Scene? map_2 = MapManager.instance.GetNextMap();
        if (map_1 == null || map_2 == null) return;

        GetBackgroundData((Scene)map_1, (Scene)map_2);
        if (background[0].Count == 0 || background[1].Count == 0) return;

        ChooseBackgroundToLoad();
        InitFadeInBackground();

        activated_corutine.Add(
            StartCoroutine(
                FadeInOut(background_array[fade_index[(int)FADE.IN]],
                          background_array[fade_index[(int)FADE.OUT]])));
        is_activated = true;
    }

    private void GetBackgroundData(Scene map_1, Scene map_2)
    {
        SpriteRenderer[] renderers;
        GameObject obj;
        GameObject[][] objects = new GameObject[2][];

        objects[0] = map_1.GetRootGameObjects();
        objects[1] = map_2.GetRootGameObjects();
        map_build_index[0] = map_1.buildIndex;
        map_build_index[1] = map_2.buildIndex;

        if (!background_position.ContainsKey(map_1.buildIndex)) 
            background_position.Add(map_1.buildIndex, new List<Vector3>());
        if (!background_position.ContainsKey(map_2.buildIndex))
            background_position.Add(map_2.buildIndex, new List<Vector3>());

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < objects[i].Length; j++)
            {
                obj = objects[i][j];
                if (obj.tag == "Background")
                {
                    background_object[i].Add(obj);
                    renderers = obj.GetComponentsInChildren<SpriteRenderer>();
                    background_counter[i].Add(renderers.Length);
                    foreach (SpriteRenderer ele in renderers)
                    {
                        background[i].Add(ele);
                    }
                }
            }
            object_array[i] = background_object[i].ToArray();
            background_array[i] = background[i].ToArray();
        }
    }

    private void ChooseBackgroundToLoad()
    {
        Vector3 player_velocity = player_ptr.GetComponent<Rigidbody2D>().velocity;
        Vector3 player_endpoint_vec = player_ptr.transform.position -
            gameObject.GetComponentInParent<CheckPoint>().transform.position;
        if (Vector3.Dot(player_velocity, player_endpoint_vec) > 0)
        {
            fade_index[(int)FADE.IN] = 1;
            fade_index[(int)FADE.OUT] = 0;
            int temp = map_build_index[0];
            map_build_index[0] = map_build_index[1];
            map_build_index[1] = temp;
        }
        else
        {
            fade_index[(int)FADE.IN] = 0;
            fade_index[(int)FADE.OUT] = 1;
        }
    }

    private void InitFadeInBackground()
    {
        int counter = 0;
        SpriteRenderer renderer;
        List<Vector3> positions = background_position[map_build_index[(int)FADE.IN]];
        bool position_should_load = positions.Count > 0 ? true : false;

        for (int i = 0; i < object_array[fade_index[(int)FADE.IN]].Length; i++)
        {
            object_array[fade_index[(int)FADE.IN]][i].SetActive(true);
            for (int j = 0; j < background_counter[fade_index[(int)FADE.IN]][i]; j++)
            {
                renderer = background_array[fade_index[(int)FADE.IN]][counter];
                ScrollingBackground scroller = renderer.GetComponent<ScrollingBackground>();
                if (scroller == null)
                {
                    scroller = renderer.GetComponentInParent<ScrollingBackground>();
                    if (scroller == null) return;
                }

                Color color = renderer.color;
                color.a = 0.0f;
                renderer.color = color;

                if (!position_should_load)
                {
                    Vector3 pos_diff = renderer.transform.position - player_ptr.transform.position;
                    pos_diff *= scroller.GetMultiplier();
                    renderer.transform.position -= pos_diff;
                }
                else renderer.transform.position = positions[counter];
                ++counter;
            }
        }

        if (position_should_load) positions.Clear();
    }

    private void CleanUpFadeOutBackground()
    {
        int counter = 0;
        SpriteRenderer renderer;
        List<Vector3> positions = background_position[map_build_index[(int)FADE.OUT]];

        for (int i = 0; i < object_array[fade_index[(int)FADE.OUT]].Length; i++)
        {
            for (int j = 0; j < background_counter[fade_index[(int)FADE.OUT]][i]; j++)
            {
                renderer = background_array[fade_index[(int)FADE.OUT]][counter];
                positions.Add(renderer.transform.position);
                ++counter;
            }
            object_array[fade_index[(int)FADE.OUT]][i].SetActive(false);
        }
    }

    private IEnumerator FadeInOut(SpriteRenderer[] fade_in, SpriteRenderer[] fade_out)
    {
        int update_count = 0;

        if (fade_out.Length > 0)
        {
            update_count = (int)(1.0f / fade_interval * (1 - overlap_amount));
            foreach (SpriteRenderer renderer in fade_out)
            {
                activated_corutine.Add(StartCoroutine(FadeOut(renderer)));
            }
        }

        while (update_count > 0)
        {
            update_count--;
            yield return new WaitForSeconds(update_interval);
        }

        if (fade_in.Length > 0)
        {
            update_count = (int)(1.0f / fade_interval);
            foreach (SpriteRenderer renderer in fade_in)
            {
                activated_corutine.Add(StartCoroutine(FadeIn(renderer)));
            }
        }

        while (update_count > 0)
        {
            update_count--;
            yield return new WaitForSeconds(update_interval);
        }

        is_activated = false;
        CleanUp();
    }

    private IEnumerator FadeIn(SpriteRenderer renderer)
    {
        float alpha = renderer.color.a;
        Color color = renderer.color;

        while (renderer.color.a < 1.0f)
        {
            alpha += fade_interval;
            color.a = alpha;
            renderer.color = color;
            yield return new WaitForSeconds(update_interval);
        }
    }

    private IEnumerator FadeOut(SpriteRenderer renderer)
    {
        float alpha = renderer.color.a;
        Color color = renderer.color;

        while (renderer.color.a > 0)
        {
            alpha -= fade_interval;
            color.a = alpha;
            renderer.color = color;
            yield return new WaitForSeconds(update_interval);
        }
    }

    private void CleanUp()
    {
        CleanUpFadeOutBackground();
        background[0].Clear();
        background[1].Clear();
        background_object[0].Clear();
        background_object[1].Clear();
    }
}

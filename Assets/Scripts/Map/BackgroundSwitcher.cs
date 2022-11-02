using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSwitcher : MonoBehaviour
{
    public float update_interval = 0.016f;
    public float fade_interval = 0.01f;
    public float overlap_amount = 0.5f;

    private bool is_activated = false;
   
    private List<Coroutine> activated_corutine = new List<Coroutine>();
    private List<SpriteRenderer>[] background = new List<SpriteRenderer>[2];
    private List<GameObject>[] background_object = new List<GameObject>[2];

    private GameObject player_ptr;
    private GameObject[] fade_in_obj;
    private SpriteRenderer[] fade_in;
    private SpriteRenderer[] fade_out;

    private void Start()
    {
        background[0] = new List<SpriteRenderer>();
        background[1] = new List<SpriteRenderer>();
        background_object[0] = new List<GameObject>();
        background_object[1] = new List<GameObject>();
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

        foreach (GameObject obj in ((Scene)map_1).GetRootGameObjects())
        {
            if (obj.tag == "Background")
            {
                background_object[0].Add(obj);
                foreach (var ele in obj.GetComponentsInChildren<SpriteRenderer>())
                {
                    background[0].Add(ele);
                }
            }
        }

        foreach (GameObject obj in ((Scene)map_2).GetRootGameObjects())
        {
            if (obj.tag == "Background")
            {
                background_object[1].Add(obj);
                foreach (var ele in obj.GetComponentsInChildren<SpriteRenderer>())
                {
                    background[1].Add(ele);
                }
            }
        }

        if (background[0].Count == 0 || background[1].Count == 0) return;

        Vector3 player_velocity = player_ptr.GetComponent<Rigidbody2D>().velocity;
        Vector3 player_endpoint_vec = player_ptr.transform.position -
            gameObject.GetComponentInParent<CheckPoint>().transform.position;
        if (Vector3.Dot(player_velocity, player_endpoint_vec) > 0)
        {
            fade_in = background[1].ToArray();
            fade_out = background[0].ToArray();
            fade_in_obj = background_object[1].ToArray();
        }
        else
        {
            fade_in = background[0].ToArray();
            fade_out = background[1].ToArray();
            fade_in_obj = background_object[0].ToArray();
        }

        foreach (GameObject obj in fade_in_obj)
        {
            obj.SetActive(true);
            foreach (SpriteRenderer renderer in fade_in)
            {
                ScrollingBackground scroller = renderer.GetComponent<ScrollingBackground>();
                if (scroller == null)
                {
                    scroller = renderer.GetComponentInParent<ScrollingBackground>();
                    if (scroller == null) return;
                }

                Color color = renderer.color;
                color.a = 0.0f;
                renderer.color = color;

                Vector3 pos_diff = renderer.transform.position - player_ptr.transform.position;
                pos_diff *= scroller.GetMultiplier();
                renderer.transform.position -= pos_diff;
            }
        }
        

        activated_corutine.Add(StartCoroutine(FadeInOut(fade_in, fade_out)));
        is_activated = true;
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
        background[0].Clear();
        background[1].Clear();
        background_object[0].Clear();
        background_object[1].Clear();
    }
}

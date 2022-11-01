using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundSwitcher : MonoBehaviour
{
    public float update_interval = 0.016f;
    public float fade_interval = 0.01f;

    private bool is_activated = false;
    private bool has_passed = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (is_activated)
        {
            if(has_passed) has_passed = false;
            else has_passed = true;
            return;
        }

        Scene? current = MapManager.instance.GetCurrentMap();
        Scene? next = MapManager.instance.GetNextMap();
        if (current == null || next == null) return;

        Scene fade_in_map, fade_out_map;
        List<SpriteRenderer> fade_out = new List<SpriteRenderer>();
        List<SpriteRenderer> fade_in = new List<SpriteRenderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (!has_passed)
        {
            fade_out_map = (Scene)current;
            fade_in_map = (Scene)next;
        }
        else
        {
            fade_in_map = (Scene)current;
            fade_out_map = (Scene)next;
        }

        foreach (GameObject obj in fade_out_map.GetRootGameObjects())
        {
            if (obj.tag.Contains("Background"))
            {
                foreach (var ele in obj.GetComponentsInChildren<SpriteRenderer>())
                {
                    fade_out.Add(ele);
                }
            }
        }

        foreach (GameObject obj in fade_in_map.GetRootGameObjects())
        {
            if (obj.tag == "Background")
            {
                if (!has_passed)
                {
                    obj.SetActive(true);
                    Vector3 position = obj.transform.position;
                    position.x = player.transform.position.x;
                    obj.transform.position = position;
                }

                foreach (var ele in obj.GetComponentsInChildren<SpriteRenderer>())
                {
                    Color color = ele.color;
                    color.a = 0.0f;
                    ele.color = color;
                    fade_in.Add(ele);
                }
            }
        }

        StartCoroutine(FadeInOut(fade_in.ToArray(), fade_out.ToArray()));

        is_activated = true;
        if (has_passed) has_passed = false;
        else has_passed = true;
    }

    private IEnumerator FadeInOut(SpriteRenderer[] fade_in, SpriteRenderer[] fade_out)
    {
        int update_count = 0;

        if (fade_out.Length > 0)
        {
            update_count = (int)(fade_out[0].color.a / fade_interval);
            foreach (SpriteRenderer renderer in fade_out)
            {
                StartCoroutine(FadeOut(renderer));
            }
        }

        while (update_count > 0)
        {
            update_count--;
            yield return new WaitForSeconds(update_interval);
        }

        if (fade_in.Length > 0)
        {
            update_count = (int)(fade_in[0].color.a / fade_interval);
            foreach (SpriteRenderer renderer in fade_in)
            {
                StartCoroutine(FadeIn(renderer));
            }
        }

        while (update_count > 0)
        {
            update_count--;
            yield return new WaitForSeconds(update_interval);
        }

        is_activated = false;
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
}

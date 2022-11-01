using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private bool is_map_loaded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (is_map_loaded) return;
        if (collision.gameObject.tag != "Player") return;

        MapManager.instance.LoadMap();
        is_map_loaded = true;
    }
}

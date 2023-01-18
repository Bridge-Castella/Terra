using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject trail;

    private GameObject parentObject;

    private void Start()
    {
        parentObject = transform.parent.gameObject;
    }

    private void Update()
    {
        if (timeBtwSpawns <= 0)
        {
            GameObject instance = (GameObject)Instantiate(trail, transform.position, Quaternion.identity);
            Destroy(instance, 2f);
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}

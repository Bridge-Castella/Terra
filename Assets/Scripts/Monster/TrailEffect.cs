using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject trail;

    private GameObject parentObject;
    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        parentObject = transform.parent.gameObject;
    }

    private void Update()
    {
        if (timeBtwSpawns <= 0)
        {
            GameObject instance = (GameObject)Instantiate(trail, transform.position, Quaternion.identity);
            Destroy(instance, 2f);
            timeBtwSpawns = startTimeBtwSpawns;

            // TODO: 거리에 따른 볼륨 조절
            // 거리 안에 들어가면 100%의 소리로 나오는것도 이상하고, 보이지도 않는 몬스터의 소리가 게임 내내 나는것도 이상함
            if (isInCamera())
            {
                InGameAudio.Post(InGameAudio.Instance.inGame_Monster_grass);
            }
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }

    private bool isInCamera()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            return true;
        }

        return false;
    }
}

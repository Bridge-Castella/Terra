using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject trail;

    public float grassSoundMinThreshold = 15f;
    public float grassSoundMaxThreshold = 10f;

    private GameObject parentObject;
    private Transform player;

    private bool isAudioPlaying = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        parentObject = transform.parent.gameObject;
    }

    private void Update()
    {
        if (isAudioPlaying && Home.IsHomeActive)
        {
            InGameAudio.Stop(InGameAudio.Instance.inGame_Monster_grass);
        }

        if (timeBtwSpawns <= 0)
        {
            GameObject instance = (GameObject)Instantiate(trail, transform.position, Quaternion.identity);
            Destroy(instance, 2f);
            timeBtwSpawns = startTimeBtwSpawns;

            if (isInCamera())
            {
                isAudioPlaying = true;
                InGameAudio.Post(InGameAudio.Instance.inGame_Monster_grass);
            }
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }

        if (isInCamera())
        {
            var cameraPos = Camera.main.transform.position;
            var distance = Vector2.Distance(cameraPos, transform.parent.position);
            
            if (distance < grassSoundMaxThreshold)
            {
                AudioManager.instance.SetMonsterGrassVolume(1.0f);
            }
            else
            {
                float diff = distance - grassSoundMaxThreshold;
                float volume = 1 - (diff / (grassSoundMinThreshold - grassSoundMaxThreshold));
                AudioManager.instance.SetMonsterGrassVolume(volume);
            }
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

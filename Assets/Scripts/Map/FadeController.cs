using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
	public GameObject[] waypoints;
	private PlayerMove player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            Vector3 waypoint = new Vector3();
            Vector3 player_pos = player.transform.position;
            float max_distance = 0f;

            foreach (var candidate in waypoints)
            {
                Vector3 candidate_pos = candidate.transform.position;
                float distance = Vector3.Distance(player_pos, candidate_pos);

                if (distance > max_distance)
                {
                    waypoint = candidate_pos;
                    max_distance = distance;
                }
            }

            player.waypoint = waypoint;
            player.onSceneChange = true;

            if (SceneFader.instance != null)
                SceneFader.instance.FadeOut();
        }
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.onSceneChange = false;

        if (SceneFader.instance != null)
            SceneFader.instance.FadeIn();
    }
}

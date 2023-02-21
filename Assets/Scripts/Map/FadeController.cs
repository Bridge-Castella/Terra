using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
	public bool initializePosition = true;
	public GameObject[] environment;
    public GameObject[] waypoints;

    private bool firstLoad = true;
    private PlayerMove player;

	private void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (firstLoad)
            {
				firstLoad = false;
				foreach (GameObject env in environment)
				{
					if (!env.activeSelf)
						env.SetActive(true);

					if (initializePosition)
					{
						var bg = env.GetComponentsInChildren<ScrollingBackground>();
						foreach (ScrollingBackground ele in bg)
						{
							ele.InitPosition();
						}
					}
				}
			}

			player.onSceneChange = false;
			SceneFader.instance.FadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
			Vector3 waypoint = new Vector3();
			Vector3 player_pos = player.transform.position;
			float min_distance = float.MaxValue;

			foreach (var candidate in waypoints)
			{
				Vector3 candidate_pos = candidate.transform.position;
				float distance = Vector3.Distance(player_pos, candidate_pos);

                if (distance < min_distance)
				{
					waypoint = candidate_pos;
					min_distance = distance;
				}
			}

			player.waypoint = waypoint;
			player.onSceneChange = true;
			SceneFader.instance.FadeOut();
        }
    }
}

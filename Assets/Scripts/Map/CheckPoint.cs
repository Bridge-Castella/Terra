using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private FallDetector fallDetector;
    private Animator animator;
    //마지막 지점이라는 것을 표시
    //public bool isEndPoint = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        fallDetector = FindObjectOfType<FallDetector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fallDetector.CheckPoint = gameObject.transform;
            //animator.SetTrigger("Move");

            // Unload map 방지
            SubmitCheckPoint();
        }
        GetComponent<Collider2D>().enabled = false;
    }

    private void SubmitCheckPoint()
    {
        int sceneIndex = gameObject.scene.buildIndex;
        MapManager.MapIndex mapIndex = MapManager.ToMapIndex(sceneIndex);

        // 이전 Check Point가 위치한 map unload
        if (MapManager.state.checkPoint != MapManager.MapIndex.Login
            && MapManager.state.checkPoint != mapIndex)
            MapManager.UnloadMap(MapManager.state.checkPoint);

        // 새로운 check point 등록
        MapManager.state.checkPoint = mapIndex;
    }
}

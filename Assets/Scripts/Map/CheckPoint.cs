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
        if (LoginManager.IsGameLoaded)
        {
            LoginManager.IsGameLoaded = false;
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            fallDetector.CheckPoint = gameObject.transform;
            //animator.SetTrigger("Move");

            // 현재 check point 등록
            SubmitCheckPoint();

            // CheckPoint 도달시 save
            SaveManager.SaveGame();
        }
        GetComponent<Collider2D>().enabled = false;
    }

    private void SubmitCheckPoint()
    {
        // 현재 Map index 정보
        int sceneIndex = gameObject.scene.buildIndex;
        MapManager.MapIndex mapIndex = MapManager.ToMapIndex(sceneIndex);

        // 새로운 check point 등록
        MapManager.MapIndex previous = MapManager.state.checkPoint;
        MapManager.state.checkPoint = mapIndex;
        ControlManager.instance.startPoint = transform.position;

        // 이전 Check Point가 위치한 map unload
        if (MapManager.state.checkPoint != MapManager.MapIndex.Login
            && MapManager.TryRemoveFromOutOfRange(previous))
                MapManager.UnloadMap(previous);

        InGameAudio.Post(InGameAudio.Instance.inGame_SavePoint);
    }
}

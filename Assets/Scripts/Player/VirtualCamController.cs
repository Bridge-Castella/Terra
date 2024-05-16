using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamController : MonoBehaviour
{
    [SerializeField]
    private bool isPassThrough = false;

    public static VirtualCamController Current;
    public GameObject virtualCam;

    private CinemachineVirtualCamera cinemachineCam;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cinemachineCam = virtualCam.GetComponent<CinemachineVirtualCamera>();
        cinemachineCam.Follow = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (isPassThrough)
            {
                InGameAudio.Stop(MapStateChanger.CurrentMapBGM);
                InGameAudio.Post(InGameAudio.Instance.BGM_Transition_loop);
            }
            virtualCam.SetActive(true);
            Current = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (isPassThrough)
            {
                InGameAudio.Stop(InGameAudio.Instance.BGM_Transition_loop);
            }
            virtualCam.SetActive(false);
            Current = null;
        }
    }
}

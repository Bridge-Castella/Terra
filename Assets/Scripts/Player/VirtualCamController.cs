using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamController : MonoBehaviour
{
    public GameObject virtualCam;

    private void Awake()
    {
        Transform player_tf = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCam.GetComponent<CinemachineVirtualCamera>().Follow = player_tf;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}

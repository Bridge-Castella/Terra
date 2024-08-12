using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnFirstLoad : MonoBehaviour
{
    [Header("게임 처음 로드시 필수적으로 활성화 되어야하는 항목들")]
    [SerializeField] private GameObject[] objects;
    static bool firstLoad = true;

    private void Start()
    {
        if (firstLoad)
        {
            firstLoad = false;
            foreach (var obj in objects)
                obj.SetActive(true);
        }
    }
}

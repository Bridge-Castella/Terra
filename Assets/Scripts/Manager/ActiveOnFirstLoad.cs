using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnFirstLoad : MonoBehaviour
{
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

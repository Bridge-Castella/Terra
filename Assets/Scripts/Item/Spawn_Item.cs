using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Item : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab_obj;

    void Start()
    {
        GameObject obj = MonoBehaviour.Instantiate(prefab_obj);
        obj.transform.position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

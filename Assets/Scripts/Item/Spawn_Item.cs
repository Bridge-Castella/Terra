using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Item : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab_obj;
    private bool isOnce =true;
    public ParticleSystem particleObject;

    void Start()
    {
        particleObject.Pause();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (isOnce)
            {
                GameObject obj = MonoBehaviour.Instantiate(prefab_obj);
                obj.transform.position = gameObject.transform.position;
                isOnce = false;
                particleObject.Play();
                Destroy(gameObject, 1.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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
        if (collision.CompareTag("Ladder"))
        {
            if (isOnce)
            {
                LadderOpacity Ladder = collision.gameObject.GetComponent<LadderOpacity>();
                //Ladder.bIsLadderUpdate =true;
                Ladder.ShowLadderOpacity();
                isOnce = false;
                particleObject.Play();
                Destroy(gameObject, 1.0f);
            }
        }
        else
        {
            particleObject.Play();
            Destroy(gameObject, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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
            LadderOpacity Ladder = collision.gameObject.GetComponent<LadderOpacity>();
            StartCoroutine(Ladder.CoShowLadderOpacity());
        }

        particleObject.Play();
        Destroy(gameObject, 1.0f);
    }
}

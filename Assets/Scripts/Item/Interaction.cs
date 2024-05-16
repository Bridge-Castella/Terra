using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private bool enableInteraction;
    protected PlayerAbilityTracker abilities;

    private enum InteractionType
    {
        Bonfire,

    }

    [SerializeField] private InteractionType type;

    protected virtual void Update()
    {
        if(enableInteraction)
        {
            if (Input.GetButtonDown("TalktoNpc"))
            {
                switch (type)
                {
                    case InteractionType.Bonfire:
                        InteractionWithBonfire();
                        break;
                }
            }
        }
    }

    //트리거 안에 들어가면 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enableInteraction = true;
            abilities = collision.GetComponent<PlayerAbilityTracker>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enableInteraction = false;
        }
    }

    public virtual void InteractionWithBonfire()
    {
    }
}

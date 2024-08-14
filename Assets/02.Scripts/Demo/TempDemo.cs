using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TempDemo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GNBCanvas.instance.CreditCanvas.FadeCredit(GNBCanvas.instance.Header);
        }
    }
}

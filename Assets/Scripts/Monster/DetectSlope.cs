using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSlope : MonoBehaviour
{

    public float angle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        angle = Vector2.Angle(Vector2.up, collision.GetContact(0).normal);
    }
}

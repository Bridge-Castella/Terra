using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LadderOpacity : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    private SpriteShapeRenderer renderer;

    private Color color;
    
    void Start()
    {
        renderer = GetComponentInParent<SpriteShapeRenderer>();
        color = new Color();
        color = Color.white;
        color.a = 0;
        renderer.color = color;
    }

    public IEnumerator CoShowLadderOpacity()
    {
        particleSystem.gameObject.SetActive(false);
        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            renderer.color = color;
            yield return null;
        }
    }
}

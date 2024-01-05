using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LadderOpacity : MonoBehaviour
{
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
        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            renderer.color = color;
            yield return null;
        }
    }
}

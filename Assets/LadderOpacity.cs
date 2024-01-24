using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LadderOpacity : MonoBehaviour
{
    [SerializeField] int ladderIndex;
    [SerializeField] private ParticleSystem particleSystem;
    private SpriteShapeRenderer renderer;

    private Color color;
    public bool IsEnabled { get; private set; }
    
    void Start()
    {
        renderer = GetComponentInParent<SpriteShapeRenderer>();
        color = new Color();
        color = Color.white;
        color.a = 0;
        renderer.color = color;
        IsEnabled = false;

        if (!GlobalContainer.contains("Ladder") ||
            GlobalContainer.load<bool[]>("Ladder") == null)
        {
            var newData = new bool[ladderIndex + 1];
            for (int i = 0; i < newData.Length; ++i)
            {
                newData[i] = false;
            }

            GlobalContainer.store("Ladder", newData);
            return;
        }

        var saveData = GlobalContainer.load<bool[]>("Ladder");
        if (saveData.Length - 1 < ladderIndex)
        {
            var newData = new bool[ladderIndex + 1];
            Array.Copy(saveData, newData, saveData.Length);
            GlobalContainer.store("Ladder", newData);
            return;
        }

        if (saveData[ladderIndex])
        {
            particleSystem.gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
            color.a = 1f;
            renderer.color = color;
            IsEnabled = true;
        }
    }

    public IEnumerator CoShowLadderOpacity()
    {
        particleSystem.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        
        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            renderer.color = color;
            yield return null;
        }

        color.a = 1f;
        renderer.color = color;
        IsEnabled = true;

        var saveData = GlobalContainer.load<bool[]>("Ladder");
        saveData[ladderIndex] = true;
        GlobalContainer.store("Ladder", saveData);
    }
}

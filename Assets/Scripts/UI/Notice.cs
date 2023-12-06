using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using TreeEditor;

public class Notice : MonoBehaviour
{
    public static Notice instance;

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textMesh;

    [Header("Settings")]
    [SerializeField] float duration = 0.5f;
    [SerializeField] float delay = 2f;
    [SerializeField] Vector2 moveDirection;
 
    struct Data
    {
        public Sprite sprite;
        public string text;
    }

    private Queue<Data> noticeQueue;
    private Vector3 initialPos;

    private bool isActive;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        noticeQueue = new Queue<Data>();
        initialPos = image.transform.position;
    }

    public void Show(string path, string text)
    {
        Show(Resources.Load<Sprite>(path), text);
    }

    public void Show(Sprite sprite, string text)
    {
        noticeQueue.Enqueue(new Data() { 
            sprite = sprite,
            text = text
        });

        if (!isActive)
        {
            StartCoroutine(Activate());
        }
    }

    public void Test()
    {
        Show(null as Sprite, "아이템을 획득하였습니다.");
    }

    private IEnumerator Activate()
    {
        isActive = true;
        image.gameObject.SetActive(true);
        textMesh.gameObject.SetActive(true);

        var data = noticeQueue.Dequeue();
        var timeElapsed = 0f;
        var scale = 0f;

        transform.position = initialPos - (Vector3)moveDirection;
        image.color = new Color(1f, 1f, 1f, 0f);
        textMesh.color = new Color(1f, 1f, 1f, 0f);
        image.sprite = data.sprite;
        textMesh.text = data.text;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            scale = Mathf.Lerp(0f, 1f, timeElapsed / duration);

            Color color = Color.white;
            color.a = scale;
            Vector3 dir = (1 - scale) * moveDirection;

            image.color = color;
            textMesh.color = color;
            transform.position = initialPos - dir;

            yield return null;
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        var timeElapsed = 0f;
        var scale = 0f;

        image.color = Color.white;
        textMesh.color = Color.white;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            scale = Mathf.Lerp(0f, 1f, timeElapsed / duration);

            Color color = Color.white;
            color.a = 1f - scale;
            Vector3 dir = scale * moveDirection;

            image.color = color;
            textMesh.color = color;
            transform.position = initialPos + dir;

            yield return null;
        }

        if (noticeQueue.Count > 0)
        {
            StartCoroutine(Activate());
        }
        else
        {
            isActive = false;
            transform.position = initialPos;
            image.gameObject.SetActive(false);
            textMesh.gameObject.SetActive(false);
        }
    }
}
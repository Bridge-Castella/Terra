using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notice : MonoBehaviour
{
    public static Notice instance;

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
        initialPos = transform.position;
    }

    public void Show(string path, string text)
    {
        Show(text);
    }

    public void Show(string text)
    {
        noticeQueue.Enqueue(new Data() { 
            text = text
        });

        if (!isActive)
        {
            StartCoroutine(Activate());
        }
    }

    public void Test()
    {
        Show("아이템을 획득하였습니다.");
    }

    private IEnumerator Activate()
    {
        isActive = true;
        textMesh.gameObject.SetActive(true);

        var data = noticeQueue.Dequeue();
        var timeElapsed = 0f;
        var scale = 0f;

        transform.position = initialPos - (Vector3)moveDirection;
        textMesh.color = new Color(1f, 1f, 1f, 0f);
        textMesh.text = data.text;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            scale = Mathf.Lerp(0f, 1f, timeElapsed / duration);

            Color color = Color.white;
            color.a = scale;
            Vector3 dir = (1 - scale) * moveDirection;

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

        textMesh.color = Color.white;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            scale = Mathf.Lerp(0f, 1f, timeElapsed / duration);

            Color color = Color.white;
            color.a = 1f - scale;
            Vector3 dir = scale * moveDirection;

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
            textMesh.gameObject.SetActive(false);
        }
    }
}
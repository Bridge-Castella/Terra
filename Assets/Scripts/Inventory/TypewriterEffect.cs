using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 10f;
    public bool isTyping;
    public void Run(string textToType, TextMeshProUGUI textLabel)
    {
        StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TextMeshProUGUI textLabel)
    {
        float t = 0;
        int charIndex = 0;

        while(charIndex < textToType.Length)
        {
            if (Input.GetButtonDown("TalktoNpc") && charIndex > 3)
            {
                break;
            }
            isTyping = true;
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
        isTyping = false;
        textLabel.text = textToType;
        GetComponent<Dialogue>().ShowAnswerButton();
    }
}

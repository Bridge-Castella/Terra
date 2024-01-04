using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 10f;
    public bool isTyping;
    public void Run(string npcDiffID, string textToType, TextMeshProUGUI textLabel)
    {
        StartCoroutine(TypeText(npcDiffID, textToType, textLabel));
    }

    private IEnumerator TypeText(string npcDiffID, string textToType, TextMeshProUGUI textLabel)
    {
        float t = 0;
        int charIndex = 0;

        switch (npcDiffID)
        {
            case "rato":
                InGameAudio.Post(InGameAudio.Instance.inGame_NPC_Rato);
                break;

            case "riche":
                InGameAudio.Post(InGameAudio.Instance.inGame_NPC_Riche);
                break;
        }

        while(charIndex < textToType.Length)
        {
            if (Input.GetKeyDown(KeyCode.E) && charIndex > 3)
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

        switch (npcDiffID)
        {
            case "rato":
                InGameAudio.Stop(InGameAudio.Instance.inGame_NPC_Rato);
                break;

            case "riche":
                InGameAudio.Stop(InGameAudio.Instance.inGame_NPC_Riche);
                break;
        }

        isTyping = false;
        textLabel.text = textToType;
        GetComponent<Dialogue>().ShowAnswerButton();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAction : MonoBehaviour
{
    public GameObject dialogueUIObject;
    public GameObject dialogueUiObjectInstance;
    //npc �� ��ġ
    public GameObject dialogueUIPosition;

    private Canvas canvas;
    private RectTransform dialogueUIRectTranform;

    bool isUICreated = false;

    private void Start()
    {
        dialogueUIRectTranform = dialogueUIObject.GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
    }

    public void ShowDialogueUIObject()
    {
        //ui�� ������� �ִٸ� ��������.
        if(!isUICreated)
        {
            //npc�� �ִ� ��ġ �����ͼ� ��ǳ�� ��� https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 pos = dialogueUIPosition.transform.position;  // get the game object position
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
            Vector2 canvasPosition = new Vector2
                                            (((viewportPoint.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                                             ((viewportPoint.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            dialogueUIRectTranform.anchoredPosition = canvasPosition;

            dialogueUiObjectInstance = Instantiate(dialogueUIObject, canvas.transform);
            dialogueUiObjectInstance.GetComponent<Dialogue>().DialogueWithNPC();
            isUICreated = true;
        }
    }
}

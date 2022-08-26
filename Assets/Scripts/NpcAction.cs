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

    private List<string> story_idList;
    int story_idIdx = 0;

    private void Start()
    {
        dialogueUIRectTranform = dialogueUIObject.GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        story_idList = new List<string>(TableData.instance.GetMainDataDic().Keys);
    }

    public void ShowDialogueUIObject()
    {
        //ui�� ������� �ִٸ� ��������.
        if(null == dialogueUiObjectInstance)
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
            List<string> npc_idList = new List<string>(TableData.instance.GetMainDataDic()[story_idList[story_idIdx]].Keys);
            dialogueUiObjectInstance.GetComponent<Dialogue>().DialogueWithNPC(story_idList[story_idIdx], npc_idList[0]);
            //����Ʈ ���̶�� �ε��� �Ѿ�� ����(���丮 ������� ����)
            //����Ʈ �Ϸ� �ǰ� ����Ʈ���� �ƴ϶�� �ε��� �Ѿ
            if(!QuestManager.instance.isQuesting || 
                (!QuestManager.instance.isQuesting && QuestManager.instance.isComplete))
                story_idIdx++;
            //TODO: ����Ʈ�� �Ϸ��� �� ���ɸ� ��ȭ ������ �� �־����. �����ϵ� �����ϵ� ���� ��ȭ�� ���;� �ҵ�
        }
    }
}

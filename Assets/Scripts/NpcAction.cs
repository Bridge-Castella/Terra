using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAction : MonoBehaviour
{
    public GameObject dialogueUIObject;
    public GameObject dialogueUiObjectInstance;
    //npc 위 위치
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
        //ui가 만들어져 있다면 생성안함.
        if(null == dialogueUiObjectInstance)
        {
            //npc가 있는 위치 가져와서 말풍선 띄움 https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
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
            //퀘스트 중이라면 인덱스 넘어가지 않음(스토리 진행되지 않음)
            //퀘스트 완료 되고 퀘스트중이 아니라면 인덱스 넘어감
            if(!QuestManager.instance.isQuesting || 
                (!QuestManager.instance.isQuesting && QuestManager.instance.isComplete))
                story_idIdx++;
            //TODO: 퀘스트를 완료한 후 말걸면 대화 가능할 수 있어야함. 성공하든 실패하든 같은 대화가 나와야 할듯
        }
    }
}

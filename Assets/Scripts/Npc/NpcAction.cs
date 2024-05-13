using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAction : MonoBehaviour
{
    public string npc_diff_id;
    public GameObject dialogueUIObject;
    public GameObject dialogueUiObjectInstance;
    //npc 위 위치
    //public GameObject dialogueUIPosition;

    private Canvas canvas;
    private RectTransform dialogueUIRectTranform;
    [SerializeField] private CanvasGroup btnCanvas;

    private List<string> story_idList;
    int story_idIdx = 0;

    bool isDialogueEnd = false;
    PlayerMove player;
    Animator animator;

    public bool IsDialogueEnd
    {
        get
        {
            return isDialogueEnd;
        }
    }

    public int Stroy_idIdx
    {
        get
        {
            return story_idIdx;
        }
        set
        {
            story_idIdx = value;
        }
    }

    private void Start()
    {
        dialogueUIRectTranform = dialogueUIObject.GetComponent<RectTransform>();
        canvas = GNBCanvas.instance.GetComponent<Canvas>();
        story_idList = new List<string>(TableData.instance.GetMainDataDic(npc_diff_id).Keys);
        player = FindObjectOfType<PlayerMove>();
        animator = gameObject.GetComponent<Animator>();

		string questId = QuestManager.getNextQuestId(npc_diff_id);
		QuestState? state = QuestManager.getState(questId);
		if (state == null) return;
		QuestState questState = (QuestState)state;
        if (questState == QuestState.Doing) Stroy_idIdx = 1;
	}

    public bool ShowDialogueUIObject()
    {
        string questId = QuestManager.getNextQuestId(npc_diff_id);
        QuestState? state = QuestManager.getState(questId);

        if (state == null) return false;
        QuestState questState = (QuestState)state;

        if(isDialogueEnd || questState == QuestState.Completed)
            return false;
        //ui가 만들어져 있다면 생성안함.
        if (null == dialogueUiObjectInstance)
        {
            dialogueUIObject.SetActive(true);
            animator.SetBool("isTalking", true);
            //플레이어 방향 바라보기
            transform.localScale = new Vector3(player.transform.localScale.x * transform.localScale.x, transform.localScale.y, transform.localScale.z);

            dialogueUiObjectInstance = Instantiate(dialogueUIObject, canvas.transform);

            // Canvas 내 상단 위치
            // 상단에 위치해야 Fade Image와 Option창 아래 위치
            dialogueUiObjectInstance.transform.SetSiblingIndex(0);

            if (questState == QuestState.Succeeded)
            {
                story_idIdx = story_idList.Count -1;
                isDialogueEnd = true;
            }

            //마지막대화
            if(questState == QuestState.Failed)
            {
                story_idIdx = story_idList.Count - 1;
                isDialogueEnd = true;
			}

            List<string> npc_idList = new List<string>(TableData.instance.GetMainDataDic(npc_diff_id)[story_idList[story_idIdx]].Keys);
            dialogueUiObjectInstance.GetComponent<Dialogue>().DialogueWithNPC(story_idList[story_idIdx], npc_idList[0]);

            //퀘스트 중이라면 인덱스 넘어가지 않음(스토리 진행되지 않음)
            
            if (questState != QuestState.Doing)
            {
                if(story_idIdx != story_idList.Count-1)
                    story_idIdx++;
            }
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InGameAudio.Post(InGameAudio.Instance.inGame_Fkey);
            btnCanvas.DOFade(1f, 0.3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            btnCanvas.DOFade(0f, 0.3f);
        }
    }
}

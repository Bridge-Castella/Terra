using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public Image dialogueImg;
    public GameObject dialogueButtonGroup;

    public Button dialogueAnswer1Button;
    public Button dialogueAnswer2Button;
    public TextMeshProUGUI dialogueAnswer1Text;
    public TextMeshProUGUI dialogueAnswer2Text;

    int dialogueIdx = 0;
    int string_idIdx = 0;

    //테이블 변수들
    string answer1_connect_id;
    string answer2_connect_id;
    string conv_connect_id;
    List<string> npc_idList;
    Dictionary<string, List<TableData.MainData>> mainDataDic;
    List<TableData.MainData> list;
    string story_id;

    string convType7LastDialogue;

    NpcAction npc;

    private void Start()
    {
        dialogueAnswer1Button.onClick.AddListener(OnClickDialogueAnswer1Button);
        dialogueAnswer2Button.onClick.AddListener(OnClickDialogueAnswer2Button);

        npc = FindObjectOfType<NpcAction>();
    }

    private void Update()
    {
        if(gameObject != null && gameObject.activeSelf)
        {
            if (dialogueAnswer1Button.gameObject.activeSelf || dialogueAnswer2Button.gameObject.activeSelf)
                dialogueButtonGroup.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(dialogueImg.GetComponent<RectTransform>().rect.width / 2 + 50f, 0);
            if (Input.GetButtonDown("TalktoNpc"))
            {
                //선택지가 켜져 있다면 키를 눌러도 대화가 넘어가지 않음. 선택해야 넘어감.
                if(dialogueAnswer1Button.gameObject.activeSelf || dialogueAnswer2Button.gameObject.activeSelf)
                    return;

                //대화타입이 4, 5, 6이면 대화 종료. destroy
                if (list[string_idIdx].conv_type == 4 || 
                    list[string_idIdx].conv_type == 5 || 
                    list[string_idIdx].conv_type == 6)
                {
                    ControlManager.instance.player.GetComponent<PlayerMove>().isTalking = false;
                    npc.GetComponent<Animator>().SetBool("isTalking", false);
                    Destroy(this.gameObject);
                }

                //일반대화가 이어지며 마지막 대화가 convtype이 7일때 대사가 안나오는 상황 예외처리..
                if (dialogueText.text == convType7LastDialogue)
                {
                    ControlManager.instance.player.GetComponent<PlayerMove>().isTalking = false;
                    npc.GetComponent<Animator>().SetBool("isTalking", false);
                    Destroy(this.gameObject);
                }
                if (list[string_idIdx].conv_type == 7)
                {
                    convType7LastDialogue = TableData.instance.GetDialogue(list[string_idIdx].string_id);
                    dialogueText.text = TableData.instance.GetDialogue(list[string_idIdx].string_id);
                }

                //대화타입이 3일때(선택지를 선택하고 나서)conv_connect_id에 값을 넣어줘서 npc_id에 종속되는 대화로 넘어가도록.
                if (conv_connect_id != null)
                    DialogueWithNPC(story_id, conv_connect_id);
                else
                    DialogueWithNPC(story_id, npc_idList[dialogueIdx]); //최초에 대화할때 필요
            }
        }        
    }

    public void DialogueWithNPC(string story_id, string npc_id)
    {
        this.story_id = story_id;
        npc_idList = new List<string>(TableData.instance.GetMainDataDic()[story_id].Keys);
        mainDataDic = TableData.instance.GetMainDataDic()[story_id];
        //npc_id에 종속되는 대화를 리스트로 가져오는 과정
        list = mainDataDic[npc_id];

        dialogueText.text = TableData.instance.GetDialogue(list[string_idIdx].string_id);
        answer1_connect_id = list[string_idIdx].answer1_connect_id;
        answer2_connect_id = list[string_idIdx].answer2_connect_id;

        int conv_type = list[string_idIdx].conv_type;

        switch(conv_type)
        {
            case 2:
                //대화타입이 2이면 버튼 두개 아니면 한개가 생성되고 각각 누르면 answer1_string_id에 종속되는 대화로 넘어갈 수 있음.
                dialogueAnswer1Button.gameObject.SetActive(true);
                dialogueAnswer1Text.text = TableData.instance.GetDialogue(list[string_idIdx].answer1_string_id);

                //선택지가 하나일때
                if (answer2_connect_id != "-1")
                {
                    dialogueAnswer2Button.gameObject.SetActive(true);
                    dialogueAnswer2Text.text = TableData.instance.GetDialogue(list[string_idIdx].answer2_string_id);
                }
                break;
            case 3:
                //선택지를 선택하고 나면 conv_connect_id에 값을 넣어줘서 해당 대화로 넘어가게 함.
                conv_connect_id = list[string_idIdx].conv_connect_id;
                break;
            case 4:
                //TODO: 퀘스트 구현
                QuestManager.instance.StartQuest();
                break;
                //퀘스트는 무조건 시작하므로 5일때는 그냥 대화(아무일도 일어나지 않음)
            case 6:
                //퀘스트 포기
                QuestManager.instance.StopQuest();
                break;
            case 7:
                //퀘스트 완료, 보상받기
                QuestManager.instance.StopQuest();
                break;
        }

        //종속된 대화의 인덱스가 끝이났을때
        if (string_idIdx == list.Count - 1)
        {
            string_idIdx = 0;
        }
        else
            string_idIdx++;
    }

    void OnClickDialogueAnswer1Button()
    {
        dialogueAnswer1Button.gameObject.SetActive(false);
        dialogueAnswer2Button.gameObject.SetActive(false);
        DialogueWithNPC(story_id, answer1_connect_id);
    }

    void OnClickDialogueAnswer2Button()
    {
        dialogueAnswer1Button.gameObject.SetActive(false);
        dialogueAnswer2Button.gameObject.SetActive(false);
        DialogueWithNPC(story_id, answer2_connect_id);
    }
}

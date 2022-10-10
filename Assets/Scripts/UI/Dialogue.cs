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

    //���̺� ������
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
                //�������� ���� �ִٸ� Ű�� ������ ��ȭ�� �Ѿ�� ����. �����ؾ� �Ѿ.
                if(dialogueAnswer1Button.gameObject.activeSelf || dialogueAnswer2Button.gameObject.activeSelf)
                    return;

                //��ȭŸ���� 4, 5, 6�̸� ��ȭ ����. destroy
                if (list[string_idIdx].conv_type == 4 || 
                    list[string_idIdx].conv_type == 5 || 
                    list[string_idIdx].conv_type == 6)
                {
                    ControlManager.instance.player.GetComponent<PlayerMove>().isTalking = false;
                    npc.GetComponent<Animator>().SetBool("isTalking", false);
                    Destroy(this.gameObject);
                }

                //�Ϲݴ�ȭ�� �̾����� ������ ��ȭ�� convtype�� 7�϶� ��簡 �ȳ����� ��Ȳ ����ó��..
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

                //��ȭŸ���� 3�϶�(�������� �����ϰ� ����)conv_connect_id�� ���� �־��༭ npc_id�� ���ӵǴ� ��ȭ�� �Ѿ����.
                if (conv_connect_id != null)
                    DialogueWithNPC(story_id, conv_connect_id);
                else
                    DialogueWithNPC(story_id, npc_idList[dialogueIdx]); //���ʿ� ��ȭ�Ҷ� �ʿ�
            }
        }        
    }

    public void DialogueWithNPC(string story_id, string npc_id)
    {
        this.story_id = story_id;
        npc_idList = new List<string>(TableData.instance.GetMainDataDic()[story_id].Keys);
        mainDataDic = TableData.instance.GetMainDataDic()[story_id];
        //npc_id�� ���ӵǴ� ��ȭ�� ����Ʈ�� �������� ����
        list = mainDataDic[npc_id];

        dialogueText.text = TableData.instance.GetDialogue(list[string_idIdx].string_id);
        answer1_connect_id = list[string_idIdx].answer1_connect_id;
        answer2_connect_id = list[string_idIdx].answer2_connect_id;

        int conv_type = list[string_idIdx].conv_type;

        switch(conv_type)
        {
            case 2:
                //��ȭŸ���� 2�̸� ��ư �ΰ� �ƴϸ� �Ѱ��� �����ǰ� ���� ������ answer1_string_id�� ���ӵǴ� ��ȭ�� �Ѿ �� ����.
                dialogueAnswer1Button.gameObject.SetActive(true);
                dialogueAnswer1Text.text = TableData.instance.GetDialogue(list[string_idIdx].answer1_string_id);

                //�������� �ϳ��϶�
                if (answer2_connect_id != "-1")
                {
                    dialogueAnswer2Button.gameObject.SetActive(true);
                    dialogueAnswer2Text.text = TableData.instance.GetDialogue(list[string_idIdx].answer2_string_id);
                }
                break;
            case 3:
                //�������� �����ϰ� ���� conv_connect_id�� ���� �־��༭ �ش� ��ȭ�� �Ѿ�� ��.
                conv_connect_id = list[string_idIdx].conv_connect_id;
                break;
            case 4:
                //TODO: ����Ʈ ����
                QuestManager.instance.StartQuest();
                break;
                //����Ʈ�� ������ �����ϹǷ� 5�϶��� �׳� ��ȭ(�ƹ��ϵ� �Ͼ�� ����)
            case 6:
                //����Ʈ ����
                QuestManager.instance.StopQuest();
                break;
            case 7:
                //����Ʈ �Ϸ�, ����ޱ�
                QuestManager.instance.StopQuest();
                break;
        }

        //���ӵ� ��ȭ�� �ε����� ���̳�����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI dialogueAnswer1Text;
    public TextMeshProUGUI dialogueAnswer2Text;

    [SerializeField] int dialogueIdx = 0;

    private void Update()
    {
        if(gameObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogueWithNPC();
            }
        }        
    }

    public void DialogueWithNPC()
    {
        List<string> communicationIdList = new List<string>(TableData.instance.GetMainDataDic().Keys);
        if (dialogueIdx == communicationIdList.Count - 1)
        {
            dialogueIdx = 0;
            Destroy(this.gameObject);
        }
        //communication_id
        string id = communicationIdList[dialogueIdx];
        List<TableData.MainData> mainDataList = TableData.instance.GetMainDataList(id);
        string stringId = mainDataList[0].string_id;
        //선택지를 주는 대사라면
       /* if(TableData.instance.GetConversationType(stringId) == 2)
        {
            dialogueAnswer1Text.text = 
        }*/
        string dialogueString = TableData.instance.GetDialogue(stringId);
        dialogueText.text = dialogueString;
        dialogueIdx++;
    }
}

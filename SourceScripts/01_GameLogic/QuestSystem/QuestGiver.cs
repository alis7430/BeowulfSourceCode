using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public Quest[] quests;
    public int questNumber;

    private NPCState npcState;
    private DialogueTrigger dialogueTrigger;
    private PlayerController pc;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        dialogueTrigger = this.transform.GetComponent<DialogueTrigger>();
        npcState = this.transform.GetComponent<NPCState>();

        questNumber = 0;
    }

    public void OpenQuestWindow()
    {
        //진행중인 퀘스트가 있다면 완료시까지 퀘스트 윈도우를 열 필요가 없다.
        if (npcState.GetState() == NPC_STATE.QUEST_PROGRESSING)
            return;

        for (int i = 0; i < quests.Length; i++)
        {
            // Giver가 가지고있는 퀘스트 중 클리어 하지않은 퀘스트가 있다면
            if (quests[i].isdone == false)
            {
                //현재 퀘스트 기버가 나라고 알린 뒤 현재 퀘스트 number를 저장한다.
                QuestManager.instance.SetQuestGiver(this);
                questNumber = i;

                //퀘스트 윈도우에 현재 퀘스트 정보를 넘긴다
                QuestWindow questwindow = UIManager.Instance.questWindow.GetComponent<QuestWindow>();
                questwindow.SetQuestWindow(quests[i].title, quests[i].description,
                    quests[i].experienceReward.ToString(), quests[i].goldReward.ToString(), quests[i].itemReward);

                //퀘스트 윈도우를 연다.
                UIManager.Instance.OpenQuestWindow();
                break;
            }
        }
    }

    public void AcceptQuest()
    {
        UIManager.Instance.CloseQuestWindow();

        quests[questNumber].isActive = true;
        quests[questNumber].questGiver = this;
        
        QuestManager.instance.AddCurrentQuest(quests[questNumber]);
        QuestManager.instance.ClearQuestGiver();
    }

    public void RefuseQuest()
    {
        UIManager.Instance.CloseQuestWindow();
        QuestManager.instance.ClearQuestGiver();
    }

    public void NextDialogue()
    {
        if (dialogueTrigger != null)
            dialogueTrigger.NextDialogueNumber();
    }
    
    public void SetState(NPC_STATE state)
    {
        if(npcState != null)
            npcState.SetState(state);
    }

    public bool HasQuest()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            // Giver가 가지고있는 퀘스트 중 클리어 하지않은 퀘스트가 있다면
            if (quests[i].isdone == false)
            {
                return true;
            }
        }
        return false;
    }
}

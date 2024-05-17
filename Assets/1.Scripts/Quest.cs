using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public GameObject questPanel;
    public string questName;
    public GameObject quest;
    public GameObject mainQuest;
    public GameObject[] completeButton;
    public Text[] questText;    
    public List<QuestScriptable> questScriptables = new List<QuestScriptable>();
    public Money money;
   

    public bool isDaily;
    
    // Start is called before the first frame update
    void Start()
    {
        // 추가될 퀘스트에 대한 텍스트 편의성 제공
        // questText 배열의 길이를 기준으로 초기화 작업 수행
        int textCount = questText.Length;

        for (int i = 0; i < textCount; i++)
        {
            if (i < questScriptables.Count)
            {
                questScriptables[i].isCompleted = false;
                questScriptables[i].killed = 0;
                questScriptables[i].UpdateQuestDetail();
                questText[i].text = questScriptables[i].questDetail;
            }
        }
    }

    private void Update()
    {
        if (questScriptables[0].killed >= 1 && !questScriptables[0].isCompleted)
        {
            Debug.Log("퀘스트 완");
            questScriptables[0].CheckQuestCompletion();
            questText[0].text = questScriptables[0].questDetail;
            completeButton[0].gameObject.SetActive(true);

        }
        else if (questScriptables[1].isMainClear)
        {
            questScriptables[1].CheckQuestCompletion();
            questText[1].text = questScriptables[1].questDetail;
            completeButton[1].gameObject.SetActive(true);
            questScriptables[1].isMainClear = false;
        }
        // 튜토리얼과 메인 스토리 퀘스트 진행의 시작
        if (questScriptables[2].isTutorial) // player ontrigger를 통해 istutorial체크
        {
            questScriptables[2].CheckQuestCompletion();
            if (questScriptables[2].nextQuest == null)
            {
                questScriptables[2].nextQuest = questScriptables[3];
            }
            else if (questScriptables[2].nextQuest == questScriptables[3])
            {
                questScriptables[2].nextQuest = questScriptables[4];
            }
            else if (questScriptables[2].nextQuest == questScriptables[4])
            {
                questScriptables[2].nextQuest = questScriptables[5];
            }
            questText[2].text = questScriptables[2].questDetail;
           
            
        }
        
        //for (int i = 0; i < questScriptables.Count; i++)
        //{
        //    QuestScriptable quest = questScriptables[i];

        //    if (quest.killed >= 1 && !quest.isCompleted)
        //    {
        //        quest.CheckQuestCompletion();
        //        questText[i].text = quest.questDetail;
        //        if (quest.isCompleted)
        //        {
        //            completeButton[i].SetActive(true);
        //        }
        //        // 다음 퀘스트로 전환 후 텍스트 업데이트
        //        if (quest.nextQuest != null && quest.isCompleted)
        //        {
        //            questScriptables[i] = quest.nextQuest;
        //            questScriptables[i].UpdateQuestDetail();
        //            questText[i].text = questScriptables[i].questDetail;
        //        }
        //    }
        //}
    }
    void SetupQuestChain()
    {
        // 각 퀘스트의 nextQuest를 설정합니다.
        for (int i = 0; i < questScriptables.Count-1; i++)
        {
            if (questScriptables[i].questName.StartsWith("TutorialQuest"))
            {
                questScriptables[i].nextQuest = questScriptables[i + 1];
            }
        }
    }

    void OnMouseDown()
    {
        if (!questPanel.activeSelf)
        {
            questPanel.SetActive(true);            
        }


    }
    public void QuestAgree()
    {
        if (questPanel.activeSelf)
        {
            quest.gameObject.SetActive(true);
            questPanel.gameObject.SetActive(false);
        }

    }
    public void QuestMenuClose()
    {
        if (questPanel.activeSelf)
        {
            questPanel.gameObject.SetActive(false);
        }
    }
    public void MainQuestAgree()
    {
        if (questPanel.activeSelf)
        {
            mainQuest.gameObject.SetActive(true);
            questPanel.gameObject.SetActive(false);
        }

    }
    public void MainQuestMenuClose()
    {
        if (questPanel.activeSelf)
        {
            questPanel.gameObject.SetActive(false);
        }
    }
    public void DailyReward()
    {
        money.money += questScriptables[0].rewardCoin;
        quest.gameObject.SetActive(false);
    }
    public void MainReward()
    {
        money.money += questScriptables[1].rewardCoin;
        mainQuest.gameObject.SetActive(false);
    }
    public void TutorialReward()
    {
        money.money += questScriptables[2].rewardCoin;
        
    }
    //public void DailyQuest()
    //{
    //    string oldName = questScriptable.questName;
    //    questScriptable.questName = isDaily ? "MainQuest1" : "DailyQuest";
    //    if (oldName != questScriptable.questName)
    //    {
    //        questScriptable.UpdateQuestDetail();
    //    }
    //}

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public GameObject questPanel;
    public string questName;
    public GameObject quest;
    public GameObject mainQuest;
    public GameObject tutorialQuest;
    public GameObject[] completeButton;
    public GameObject[] mainquestButtonOff;
    public GameObject[] dailyquestButtonOff;
    public Text[] questText;
    public Text[] panelText;
    public List<QuestScriptable> questScriptables = new List<QuestScriptable>();
    public Money money;


    public bool isDaily;

    // Start is called before the first frame update
    void Start()
    {
        GameObject particleObject = GameObject.Find("Tutorial2ClearZone");
        questScriptables[4].clearZone = particleObject.gameObject;
        questScriptables[4].clearZone.gameObject.SetActive(false);

        if (questScriptables[6].isTutorial)
        {
            Destroy(tutorialQuest);
        }
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
            questScriptables[0].CheckQuestCompletion();
            questText[0].text = questScriptables[0].questDetail;
            completeButton[0].gameObject.SetActive(true);
            panelText[1].text = "자정이 지난 후 초기화 됩니다";
            for (int i = 0; i < mainquestButtonOff.Length; i++)
            {
                dailyquestButtonOff[i].gameObject.SetActive(false);
            }

        }
        else if (questScriptables[1].isMainClear)
        {
            questScriptables[1].CheckQuestCompletion();
            questText[1].text = questScriptables[1].questDetail;
            completeButton[1].gameObject.SetActive(true);
            panelText[0].text = "클리어 한 임무입니다"; // 챕터1 메인 퀘스트 완료표시
            for(int i = 0; i < mainquestButtonOff.Length; i++)
            {
                mainquestButtonOff[i].gameObject.SetActive(false);
            }
            
            //questScriptables[1].isMainClear = false;
        }
        // 튜토리얼과 메인 스토리 퀘스트 진행의 시작
        if (questScriptables[2].isTutorial) // player ontrigger를 통해 istutorial체크
        {
            if (questScriptables[2].questName == "TutorialQuest3")
            {
                completeButton[2].gameObject.SetActive(true);
                // 시작튜토리얼 진행의 마지막인 3번을 완료시 완료버튼이 등장
            }
            questScriptables[2].CheckQuestCompletion(); // 튜토리얼 다음 텍스트
            questText[2].text = questScriptables[2].questDetail;
            
        }

 
    }
    void SetupQuestChain()
    {
        // 각 퀘스트의 nextQuest를 설정합니다.
        for (int i = 0; i < questScriptables.Count - 1; i++)
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
        tutorialQuest.gameObject.SetActive(false);
        
        questScriptables[6].isTutorial = true;// 튜토리얼 진행 체크용도
        questScriptables[2].isTutorial = true; 

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

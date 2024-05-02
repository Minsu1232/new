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
        for (int i = 0; i < questScriptables.Count; i++)
        {
            questScriptables[i].isCompleted = false;
            questScriptables[i].killed = 0;
            questScriptables[i].UpdateQuestDetail();
            questText[i].text = questScriptables[i].questDetail;
            
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

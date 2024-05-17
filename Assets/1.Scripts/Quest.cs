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
        // �߰��� ����Ʈ�� ���� �ؽ�Ʈ ���Ǽ� ����
        // questText �迭�� ���̸� �������� �ʱ�ȭ �۾� ����
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
            Debug.Log("����Ʈ ��");
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
        // Ʃ�丮��� ���� ���丮 ����Ʈ ������ ����
        if (questScriptables[2].isTutorial) // player ontrigger�� ���� istutorialüũ
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
        //        // ���� ����Ʈ�� ��ȯ �� �ؽ�Ʈ ������Ʈ
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
        // �� ����Ʈ�� nextQuest�� �����մϴ�.
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

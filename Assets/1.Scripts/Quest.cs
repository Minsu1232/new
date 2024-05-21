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
            questScriptables[0].CheckQuestCompletion();
            questText[0].text = questScriptables[0].questDetail;
            completeButton[0].gameObject.SetActive(true);
            panelText[1].text = "������ ���� �� �ʱ�ȭ �˴ϴ�";
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
            panelText[0].text = "Ŭ���� �� �ӹ��Դϴ�"; // é��1 ���� ����Ʈ �Ϸ�ǥ��
            for(int i = 0; i < mainquestButtonOff.Length; i++)
            {
                mainquestButtonOff[i].gameObject.SetActive(false);
            }
            
            //questScriptables[1].isMainClear = false;
        }
        // Ʃ�丮��� ���� ���丮 ����Ʈ ������ ����
        if (questScriptables[2].isTutorial) // player ontrigger�� ���� istutorialüũ
        {
            if (questScriptables[2].questName == "TutorialQuest3")
            {
                completeButton[2].gameObject.SetActive(true);
                // ����Ʃ�丮�� ������ �������� 3���� �Ϸ�� �Ϸ��ư�� ����
            }
            questScriptables[2].CheckQuestCompletion(); // Ʃ�丮�� ���� �ؽ�Ʈ
            questText[2].text = questScriptables[2].questDetail;
            
        }

 
    }
    void SetupQuestChain()
    {
        // �� ����Ʈ�� nextQuest�� �����մϴ�.
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
        
        questScriptables[6].isTutorial = true;// Ʃ�丮�� ���� üũ�뵵
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

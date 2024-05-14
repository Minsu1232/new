using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Quest",menuName ="Quest")]
public class QuestScriptable : ScriptableObject
{
    
    public int killed;
    public string questName;
    public string questDetail;
    public bool isCompleted;
    public bool isMainClear;
    public bool isTutorial;
    public int rewardCoin;
   
     

   
    // Start is called before the first frame update


    // �߰��� ����Ʈ�� ������ ���⿡ �߰�
    public void UpdateQuestDetail()
    {
        if (questName == "DailyQuest")
        {
            questDetail = "���� ���� óġ�ϱ� " + killed + "/1";
            rewardCoin = 200;
        }
        else if (questName == "MainQuest1")
        {
            questDetail = "(����) ȭ�� â�� ��������\n  �Ұ��� óġ�ϱ�";
            rewardCoin = 1000;
        }
        else if (questName == "TutorialQuest")
        {
            isTutorial = false;
            questDetail = "��������� �������ϰ� �����ض�";
            rewardCoin = 500;
        }
       
    }
    public void CheckQuestCompletion()
    {
        if (questName == "DailyQuest" && killed >= 1) // ���÷� '1'�� �ʿ�� �ϴ� ���� ����
        {
            isCompleted = true;
            questDetail = "���� ���� óġ�ϱ� " + killed + "/1 (�Ϸ�)";  // UI ������Ʈ ȣ��
            
        }
        else if(questName == "MainQuest1" && isMainClear)
        {
            
            questDetail = "�Ұ��� óġ �Ϸ�";
        }
        else if (questName == "TutorialQuest" && killed >=1)
        {            
            questDetail = "���� �Ϸ�";
            
        }

    }

}

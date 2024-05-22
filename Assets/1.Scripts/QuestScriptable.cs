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
    public bool isPrologue;

    public GameObject clearZone;
    public QuestScriptable nextQuest;




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
            questDetail = "��������� �������ϰ� �����ض� \n\n(�� ������ ü���� �ִ� ��ü �Ǵ� \nƯ�� ������Ʈ���Ը� Ȱ�� �� �� ������ \n��Ŭ���� �ڵ� ���̹� �˴ϴ�)";
            
        }
        else if (questName == "TutorialQuest1")
        {
            isTutorial = false;
            questDetail = "���ƿ� ������ �����⸦ �ľ��ض�";
            
        }
        else if (questName == "TutorialQuest2")
        {
            isTutorial = false;
            questDetail = "��ȭ���� ���ε��� ���� ������";
            clearZone.SetActive(true);
            
        }
        else if (questName == "TutorialQuest3")
        {// �������� �����ô� �ո��� �ⳳ ���� ������ ����ϴ� ��
            isTutorial = false;
            questDetail = "�߾ӿ� �ִ� �����濡�� �ҹ��� �ľ��ض�";
            rewardCoin = 800; // ���� Ʃ�丮�� ����ȳ��� ������
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
            //questDetail = "���� �Ϸ�";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest1" && isTutorial)
        {
            //isCompleted = true;
            ////questDetail = "���� ������ �ľ� �Ϸ�";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest2" && isTutorial)
        {
            //isCompleted = true;
            //questDetail = "���ε��� �� ����� �Ϸ�";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest3")
        {
            //isCompleted = true;
            //questDetail = "������ �ҹ� �ľ� �Ϸ�";
            TransferToNextQuest();
        }
    
    }
    // �Ϸ�� ��������Ʈ�� �����ϱ� ���� �ż���
    private void TransferToNextQuest()
    {
        if (nextQuest != null)
        {
           
            questName = nextQuest.questName;
            questDetail = nextQuest.questDetail;
            rewardCoin = nextQuest.rewardCoin;
            clearZone = nextQuest.clearZone;
             killed = 0; // �ʿ��� ��� �ʱ�ȭ
            isCompleted = false;
            isMainClear = false;
            isTutorial = nextQuest.isTutorial;
            nextQuest = nextQuest.nextQuest;
            nextQuest.UpdateQuestDetail();            
            Debug.Log("���� ����Ʈ�� �̵�: " + questName);
            
        }
    }


}

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
    public bool isGuide;
    public bool isTimeCheck; // ��������Ʈ �ð� ���̺� �뵵

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
            questDetail = "��������� �������ϰ� �����ض�.)";
            
        }
        else if (questName == "TutorialQuest1")
        {
            isTutorial = false;
            questDetail = "���ƿ� ������ �����⸦ �ľ��ض� \n\n(���� ���Ǳ���� ��� \n �޴��ٿ��� Ȯ�� �����մϴ�.)";
            
        }
        else if (questName == "TutorialQuest2")
        {
            isTutorial = false;
            questDetail = "��ȭ���� ���ε��� ���� ������ \n\n(�߾� ū �ǹ��� \n ����Ʈ ���� �ǹ� �Դϴ�.)";
            clearZone.SetActive(true);
            
        }
        else if (questName == "TutorialQuest3")
        {// �������� �����ô� �ո��� �ⳳ ���� ������ ����ϴ� ��
            isTutorial = false;
            questDetail = "�߾ӿ� �ִ� �����濡�� �ҹ��� �ľ��ض� \n\n(����Ʈ���� �����Ż�� ���� \n Ŭ��� �����ϸ� �ʵ����ʹ�\n ��÷� �̿� �����մϴ�)";
            rewardCoin = 800; // ���� Ʃ�丮�� ����ȳ��� ������
        }

        else if(questName == "GuideQuest")
        {
            questDetail = "���� ������ �����ô� ����, \n ������ �� Ȯ�� �ؾ� �� ���� �ֽ��ϴ� \n �غ� �ƴٸ� ���� Ŭ���� �ּ���.";
        }
        else if (questName == "GuideQuest1")
        {
            questDetail = "���� ���� �� �߻� �Դϴ�. \n �տ� ���̴� ����ƺ� ���콺�� ��Ŭ�� ���� ���� \n ��Ŭ������ �߻� �ϼ���.";
        }
        else if (questName == "GuideQuest2")
        {
            questDetail = "���ϼ̽��ϴ�. ���� �߾� �ϴܿ� �ִ� \n ��ųâ�� ���ֽð�, ������ ��� ��ų�� \n ����� ������.";
        }
        else if (questName == "GuideQuest3")
        {
            questDetail = "�����ϴ�. ���� ��ܿ� ���ø� ���� ȭ���� \n ���¸� ��Ÿ���ϴ�. ������ ��ų�� \n ������� ���̵�â���� Ȯ�� �����մϴ�. \n ������ E�� ���� ������ �÷�������.";
        }
        else if (questName == "GuideQuest4")
        {
            questDetail = "���̵��� ���� 1�� ������ �����ϴ�. \n ������ 10ȸ �ø���, ��뵵 �����մϴ�. \n ���� ����ƺ� óġ �� �������� ���ư�����.";
        }
    }
    public void CheckQuestCompletion() // �Ϸ� ������ �ɽ� �Ϸ�ǰų� �������� �Ѿư�
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
        else if (questName == "TutorialQuest4")
        {
            //isCompleted = true;
            //questDetail = "������ �ҹ� �ľ� �Ϸ�";
            TransferToNextQuest();
        }

        if (questName == "GuideQuest" && isGuide)
        {
            isGuide = false;
            TransferToNextQuest();
        }
        else if (questName == "GuideQuest1" && isGuide)
        {
            isGuide = false;
            TransferToNextQuest();
        }
        else if (questName == "GuideQuest2" && isGuide)
        {
            isGuide = false;
            TransferToNextQuest();
        }
        else if (questName == "GuideQuest3" && isGuide)
        {
            isGuide = false;
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
            isGuide = false;
            isTutorial = nextQuest.isTutorial;
            isGuide = nextQuest.isGuide;
            nextQuest = nextQuest.nextQuest;
            nextQuest.UpdateQuestDetail();            
            Debug.Log("���� ����Ʈ�� �̵�: " + questName);
            
        }
    }


}

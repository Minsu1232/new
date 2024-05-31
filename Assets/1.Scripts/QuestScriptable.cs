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
    public bool isTimeCheck; // 일일퀘스트 시간 세이브 용도

    public GameObject clearZone;
    public QuestScriptable nextQuest;




    // Start is called before the first frame update


    // 추가할 퀘스트의 내용은 여기에 추가
    public void UpdateQuestDetail()
    {
        if (questName == "DailyQuest")
        {
            questDetail = "웨어 울프 처치하기 " + killed + "/1";
            rewardCoin = 200;
        }
        else if (questName == "MainQuest1")
        {
            questDetail = "(메인) 화약 창고에 서식중인\n  불가살 처치하기";
            rewardCoin = 1000;
        }
        else if (questName == "TutorialQuest")
        {
            isTutorial = false;
            questDetail = "폐관수련을 마무리하고 복귀해라.)";
            
        }
        else if (questName == "TutorialQuest1")
        {
            isTutorial = false;
            questDetail = "돌아온 마을의 분위기를 파악해라 \n\n(각종 편의기능은 상단 \n 메뉴바에서 확인 가능합니다.)";
            
        }
        else if (questName == "TutorialQuest2")
        {
            isTutorial = false;
            questDetail = "대화중인 상인들의 말을 엿들어라 \n\n(중앙 큰 건물은 \n 퀘스트 수주 건물 입니다.)";
            clearZone.SetActive(true);
            
        }
        else if (questName == "TutorialQuest3")
        {// 승지방은 조선시대 왕명의 출납 등의 업무를 담당하던 곳
            isTutorial = false;
            questDetail = "중앙에 있는 승지방에가 소문을 파악해라 \n\n(퀘스트류는 노란포탈을 통해 \n 클리어가 가능하며 필드사냥터는\n 상시로 이용 가능합니다)";
            rewardCoin = 800; // 최초 튜토리얼 진행안내의 마지막
        }

        else if(questName == "GuideQuest")
        {
            questDetail = "이제 밖으로 나가시는 군요, \n 나가기 전 확인 해야 할 일이 있습니다 \n 준비가 됐다면 저를 클릭해 주세요.";
        }
        else if (questName == "GuideQuest1")
        {
            questDetail = "먼저 조준 및 발사 입니다. \n 앞에 보이는 허수아비에 마우스를 우클릭 으로 조준 \n 좌클릭으로 발사 하세요.";
        }
        else if (questName == "GuideQuest2")
        {
            questDetail = "잘하셨습니다. 이제 중앙 하단에 있는 \n 스킬창을 봐주시고, 마음에 드는 스킬을 \n 사용해 보세요.";
        }
        else if (questName == "GuideQuest3")
        {
            questDetail = "좋습니다. 왼쪽 상단에 보시면 현재 화살의 \n 상태를 나타냅니다. 나머지 스킬은 \n 우측상단 가이드창에서 확인 가능합니다. \n 다음은 E를 눌러 스탯을 올려보세요.";
        }
        else if (questName == "GuideQuest4")
        {
            questDetail = "보셨듯이 스탯 1당 레벨이 오릅니다. \n 스탯을 10회 올리면, 비용도 증가합니다. \n 이제 허수아비를 처치 후 세상으로 나아가세요.";
        }
    }
    public void CheckQuestCompletion() // 완료 조건이 될시 완료되거나 다음으로 넘아감
    {
        if (questName == "DailyQuest" && killed >= 1) // 예시로 '1'을 필요로 하는 조건 설정
        {
            isCompleted = true;
            
            questDetail = "웨어 울프 처치하기 " + killed + "/1 (완료)";  // UI 업데이트 호출
           
        }
        else if(questName == "MainQuest1" && isMainClear)
        {            
            questDetail = "불가살 처치 완료";
        }
        else if (questName == "TutorialQuest" && killed >=1)
        {            
            //questDetail = "복귀 완료";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest1" && isTutorial)
        {
            //isCompleted = true;
            ////questDetail = "마을 분위기 파악 완료";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest2" && isTutorial)
        {
            //isCompleted = true;
            //questDetail = "상인들의 말 엿듣기 완료";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest3")
        {
            //isCompleted = true;
            //questDetail = "승지방 소문 파악 완료";
            TransferToNextQuest();
        }
        else if (questName == "TutorialQuest4")
        {
            //isCompleted = true;
            //questDetail = "승지방 소문 파악 완료";
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
    // 완료시 다음퀘스트로 진행하기 위한 매서드
    private void TransferToNextQuest()
    {
        if (nextQuest != null)
        {
           
            questName = nextQuest.questName;
            questDetail = nextQuest.questDetail;
            rewardCoin = nextQuest.rewardCoin;
            clearZone = nextQuest.clearZone;
             killed = 0; // 필요한 경우 초기화
            isCompleted = false;
            isMainClear = false;
            isGuide = false;
            isTutorial = nextQuest.isTutorial;
            isGuide = nextQuest.isGuide;
            nextQuest = nextQuest.nextQuest;
            nextQuest.UpdateQuestDetail();            
            Debug.Log("다음 퀘스트로 이동: " + questName);
            
        }
    }


}

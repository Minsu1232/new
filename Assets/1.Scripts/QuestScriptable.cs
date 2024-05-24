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
            questDetail = "폐관수련을 마무리하고 복귀해라 \n\n(체력이 있는 객체 또는 특정 오브젝트에 \n우클릭시 에이밍 되고 좌클릭으로 쏠 수 있습니다.)";
            
        }
        else if (questName == "TutorialQuest1")
        {
            isTutorial = false;
            questDetail = "돌아온 마을의 분위기를 파악해라 \n\n(상점은 노란색 선이 그려진 작은 \n 건물이고 클릭시 이용 가능 합니다)";
            
        }
        else if (questName == "TutorialQuest2")
        {
            isTutorial = false;
            questDetail = "대화중인 상인들의 말을 엿들어라 \n\n(선이 그려진 큰 건물은 \n 퀘스트 수주 건물 입니다.)";
            clearZone.SetActive(true);
            
        }
        else if (questName == "TutorialQuest3")
        {// 승지방은 조선시대 왕명의 출납 등의 업무를 담당하던 곳
            isTutorial = false;
            questDetail = "중앙에 있는 승지방에가 소문을 파악해라 \n\n(퀘스트류는 노란포탈을 통해 \n 클리어가 가능하며 필드사냥터는\n 상시로 이용 가능합니다)";
            rewardCoin = 800; // 최초 튜토리얼 진행안내의 마지막
        }

    }
    public void CheckQuestCompletion()
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
            isTutorial = nextQuest.isTutorial;
            nextQuest = nextQuest.nextQuest;
            nextQuest.UpdateQuestDetail();            
            Debug.Log("다음 퀘스트로 이동: " + questName);
            
        }
    }


}

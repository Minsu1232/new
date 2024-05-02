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
    public int rewardCoin;
     

   
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
       
    }

}

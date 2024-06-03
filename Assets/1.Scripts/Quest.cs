using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    public TextMeshProUGUI[] questText;
    public TextMeshProUGUI[] panelText;
    public List<QuestScriptable> questScriptables = new List<QuestScriptable>();
    public Money money;
    bool isQuestTimerActive = false;
   public bool isQuestCompleted = false;
    public bool isDaily;
    public Outline outline;
    [System.Serializable]
    public class GameData
    {
        public string questClearTime;
    }
    public static void SaveGameData(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData(); // 파일이 없을 경우 새로운 데이터 객체를 반환
    }
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
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
                //questScriptables[i].killed = 0;
                questScriptables[i].UpdateQuestDetail();
                questText[i].text = questScriptables[i].questDetail;
            }
        }
        if (questScriptables[0].isCompleted)
        {
            for (int i = 0; i < mainquestButtonOff.Length; i++)
            {
                dailyquestButtonOff[i].gameObject.SetActive(false);
            }
            panelText[1].text = "자정이 지난 후 초기화 됩니다";
        }
        else if (questScriptables[1].isMainClear) 
        {
            panelText[0].text = "완료 한 임무입니다. \n 다음 임무 : ??? (예정)";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questPanel.SetActive(false);

        }
        if (questScriptables[0].killed >= 1 && questScriptables[0].isCompleted && !questScriptables[0].isTimeCheck)
        {
            SaveQuestClearTime(); // 퀘스트 완료 시간을 저장
            questScriptables[0].isTimeCheck = true;
        }
            
        if (questScriptables[0].killed >= 1 && questScriptables[0].isCompleted)
        {
            
            if (!isQuestTimerActive)
            {
               
                InvokeRepeating("CheckQuestReset", 0, 60.0f);  // 즉시 시작하고, 60초 간격으로 반복
                
                isQuestTimerActive = true;
            }
            
            questScriptables[0].CheckQuestCompletion();
            questText[0].text = questScriptables[0].questDetail;
            completeButton[0].gameObject.SetActive(true);
            panelText[1].text = "자정이 지난 후 초기화 됩니다";
            for (int i = 0; i < mainquestButtonOff.Length; i++)
            {
                dailyquestButtonOff[i].gameObject.SetActive(false);
            }
        

        }
       
       
        if (questScriptables[1].isMainClear)
        {
            questScriptables[1].CheckQuestCompletion();
            questText[1].text = questScriptables[1].questDetail;
            completeButton[1].gameObject.SetActive(true);
            panelText[0].text = "완료 한 임무입니다. \n 다음 임무 : ??? (예정)"; // 챕터1 메인 퀘스트 완료표시
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
    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false; 
    }
    // 일일퀘스트 초기화 시간 관리
    private void CheckQuestReset() 
    {
        GameData loadedData = LoadGameData();
        DateTime questClearTime;
        if (DateTime.TryParse(loadedData.questClearTime, out questClearTime))
        {
            DateTime currentTime = DateTime.Now;
            // 로그로 현재 시간과 퀘스트 완료 시간 차이 출력
            Debug.Log("CheckQuestReset called: " + currentTime.ToString());
            double hoursPassed = (currentTime - questClearTime).TotalHours;

            Debug.Log("Time since quest cleared: " + hoursPassed + " hours");

            if (hoursPassed >= 24) // 24시간이상이 체크되면 일일퀘스트 초기화
            {
                questScriptables[0].isTimeCheck = false;
                questScriptables[0].isCompleted = false;
                questScriptables[0].killed = 0;// 조건을 되돌린 후
                questScriptables[0].UpdateQuestDetail();
                panelText[1].text = "웨어 울프 처치하기   0/1\r\n\r\n보상 : 200냥"; // 퀘스트 패널 초기화
                completeButton[0].gameObject.SetActive(false);
                for (int i = 0; i < mainquestButtonOff.Length; i++)
                {
                    dailyquestButtonOff[i].gameObject.SetActive(true); // 버튼도 초기화
                }
                questText[0].text = questScriptables[0].questDetail;
                Debug.Log("퀘스트 초기화");
                CancelInvoke("CheckQuestReset"); // 타이머 중단
                isQuestTimerActive = false;
            }
        }
        else
        {
            Debug.LogError("Invalid quest clear time stored.");
        }
    }
    private void SaveQuestClearTime()
    {
        GameData data = new GameData
        {
            questClearTime = DateTime.Now.ToString()
        };
        SaveGameData(data);
        Debug.Log(data);
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
        MoneyManager.Instance.money.money += questScriptables[0].rewardCoin;
        quest.gameObject.SetActive(false);
    }
    public void MainReward()
    {
        MoneyManager.Instance.money.money += questScriptables[1].rewardCoin;
        mainQuest.gameObject.SetActive(false);
    }
    public void TutorialReward()
    {
        MoneyManager.Instance.money.money += questScriptables[2].rewardCoin;
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

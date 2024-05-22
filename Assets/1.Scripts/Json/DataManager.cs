using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }  // 싱글톤 인스턴스 접근자

    public PlayerState playerState;
    public QuestScriptable prologue;
    public QuestScriptable tutorial;
    public QuestScriptable mainQuest;
    public QuestScriptable dailyQuest;
    public Money money;
    

    public bool isDataLoaded = false; // 데이터 로드 완료 상태 플래그
    public bool isPrologued = false;

    string PlayerDataPath;
    string QuestDataPath;
    string tutorialDataPath;
    string moneyDataPath;
    string mainDataPath;
    string dailyDataPath;
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Scene이 변경되어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);  // 싱글톤 중복 생성 방지
        }
    }
    void Start()
    {
        PlayerDataPath = Path.Combine(Application.persistentDataPath, "playerState.json");
        QuestDataPath = Path.Combine(Application.persistentDataPath, "Quest.json");
        tutorialDataPath = Path.Combine(Application.persistentDataPath, "Tutorial.json");
        moneyDataPath = Path.Combine(Application.persistentDataPath, "Money.json");
        mainDataPath = Path.Combine(Application.persistentDataPath, "MainQuest.json");
        dailyDataPath = Path.Combine(Application.persistentDataPath, "dailyQuest.json");

        PlayerLoadGameData();
        PrologueLoadGameData();
        TutorialLoadGameData();
        MoneyLoadGameData();
        MainLoadGameData();
        DailyLoadGameData();
        //InventoryLoadGameData();
    }
    [System.Serializable]
    public class ItemArray
    {
        public Item[] items;
    }
    public void PlayerSaveGameData()
    {
        string json = JsonUtility.ToJson(playerState, true);
        File.WriteAllText(PlayerDataPath, json);
        Debug.Log("Game data saved to " + PlayerDataPath);
    }
    public void PrologueSaveGameData()
    {
        string json = JsonUtility.ToJson(prologue, true);
        File.WriteAllText(QuestDataPath, json);
        Debug.Log("Game data saved to " + QuestDataPath);
    }
    public void TutorialSaveGameData()
    {
        string json = JsonUtility.ToJson(tutorial, true);
        File.WriteAllText(tutorialDataPath, json);
        Debug.Log("Game data saved to " + tutorialDataPath);
    }
    public void MoneySaveGameData()
    {
        string json = JsonUtility.ToJson(money, true);
        File.WriteAllText(moneyDataPath, json);
        Debug.Log("Inventory data saved to " + moneyDataPath);
    }
    public void MainSaveGameData()
    {
        string json = JsonUtility.ToJson(mainQuest, true);
        File.WriteAllText(mainDataPath, json);
        Debug.Log("Inventory data saved to " + mainDataPath);
    }
    public void DailySaveGameData()
    {
        string json = JsonUtility.ToJson(dailyQuest, true);
        File.WriteAllText(dailyDataPath, json);
        Debug.Log("Inventory data saved to " + dailyDataPath);
    }

    public void PlayerLoadGameData()
    {
        if (File.Exists(PlayerDataPath))
        {
            string json = File.ReadAllText(PlayerDataPath);
            JsonUtility.FromJsonOverwrite(json, playerState);
            Debug.Log("Game data loaded from " + PlayerDataPath);
            isDataLoaded = true;
        }
        //else
        //{
        //    Debug.Log("No save file found, loading default settings.");
        //    playerState = CreateInstance<PlayerState>(); // 초기 설정 로드 또는 새 인스턴스 생성
        //}
    }
    private void PrologueLoadGameData()
    {
        if (File.Exists(QuestDataPath))
        {
            string json = File.ReadAllText(QuestDataPath);
            JsonUtility.FromJsonOverwrite(json, prologue);
            Debug.Log("Prologue data loaded from " + QuestDataPath);
            isPrologued = true;
        }
    }
    private void TutorialLoadGameData()
    {
        if (File.Exists(tutorialDataPath))
        {
            string json = File.ReadAllText(tutorialDataPath);
            JsonUtility.FromJsonOverwrite(json, tutorial);
            Debug.Log("Prologue data loaded from " + tutorialDataPath);
            
        }
    }
    private void MoneyLoadGameData()
    {
        if (File.Exists(moneyDataPath))
        {
            string json = File.ReadAllText(moneyDataPath);
            JsonUtility.FromJsonOverwrite(json, money);
            Debug.Log("Prologue data loaded from " + moneyDataPath);
            
        }
    }
    private void MainLoadGameData()
    {
        if (File.Exists(mainDataPath))
        {
            string json = File.ReadAllText(mainDataPath);
            JsonUtility.FromJsonOverwrite(json, mainQuest);
            Debug.Log("Prologue data loaded from " + mainDataPath);

        }
    }
    private void DailyLoadGameData()
    {
        if (File.Exists(dailyDataPath))
        {
            string json = File.ReadAllText(dailyDataPath);
            JsonUtility.FromJsonOverwrite(json, dailyQuest);
            Debug.Log("Prologue data loaded from " + dailyDataPath);

        }
    }
    //public void InventoryLoadGameData()
    //{
    //    if (File.Exists(inventoryDataPath))
    //    {
    //        string json = File.ReadAllText(inventoryDataPath);
    //        QuestScriptableArray itemsArray = JsonUtility.FromJson<QuestScriptableArray>(json);
    //        items = itemsArray.items;
    //        Debug.Log("Inventory data loaded from " + inventoryDataPath);
    //    }
    //    else
    //    {
    //        Debug.Log("No inventory file found, loading default settings.");
    //        // 초기 설정 로드 또는 새 인스턴스 생성 코드를 여기에 추가
    //    }
    //}
    private void OnApplicationQuit()
    {// 게임을 끌때 데이터들을 저장
        PlayerSaveGameData();
        PrologueSaveGameData();
        TutorialSaveGameData();
        MoneySaveGameData();
        MainSaveGameData();
        DailySaveGameData();
        //InventorySaveGameData();
    }
}

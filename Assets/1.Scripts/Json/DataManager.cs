using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Steamworks;
public class DataManager : MonoBehaviour
{
    //20240602 제이슨저장
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
        //MoneyLoadGameData();
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
    //public void MoneySaveGameData()
    //{
    //    string json = JsonUtility.ToJson(money, true);
    //    File.WriteAllText(moneyDataPath, json);
    //    Debug.Log("Inventory data saved to " + moneyDataPath);
    //}
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
    //private void MoneyLoadGameData()
    //{
    //    if (File.Exists(moneyDataPath))
    //    {
    //        string json = File.ReadAllText(moneyDataPath);
    //        JsonUtility.FromJsonOverwrite(json, money);
    //        Debug.Log("Prologue data loaded from " + moneyDataPath);

    //    }
    //}
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
        //MoneySaveGameData();
        MainSaveGameData();
        DailySaveGameData();
        //InventorySaveGameData();
    }
}
//{ 240601 스팀클라우드 테스트
//    public static DataManager Instance { get; private set; }  // 싱글톤 인스턴스 접근자

//    public PlayerState playerState;
//    public QuestScriptable prologue;
//    public QuestScriptable tutorial;
//    public QuestScriptable mainQuest;
//    public QuestScriptable dailyQuest;
//    public Money money;

//    public bool isDataLoaded = false; // 데이터 로드 완료 상태 플래그
//    public bool isPrologued = false;

//    string PlayerDataPath;
//    string QuestDataPath;
//    string tutorialDataPath;
//    string moneyDataPath;
//    string mainDataPath;
//    string dailyDataPath;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);  // Scene이 변경되어도 파괴되지 않도록 설정
//        }
//        else
//        {
//            Destroy(gameObject);  // 싱글톤 중복 생성 방지
//        }
//    }

//    void Start()
//    {
//        PlayerDataPath = Path.Combine(Application.persistentDataPath, "playerState.json");
//        QuestDataPath = Path.Combine(Application.persistentDataPath, "Quest.json");
//        tutorialDataPath = Path.Combine(Application.persistentDataPath, "Tutorial.json");
//        moneyDataPath = Path.Combine(Application.persistentDataPath, "Money.json");
//        mainDataPath = Path.Combine(Application.persistentDataPath, "MainQuest.json");
//        dailyDataPath = Path.Combine(Application.persistentDataPath, "dailyQuest.json");

//        PlayerLoadGameData();
//        PrologueLoadGameData();
//        TutorialLoadGameData();
//        MoneyLoadGameData();
//        MainLoadGameData();
//        DailyLoadGameData();
//        InventoryLoadGameData();
//    }

//    [System.Serializable]
//    public class ItemArray
//    {
//        public Item[] items;
//    }

//    Steam 클라우드에 데이터 저장
//    public void SaveToSteamCloud(string fileName, string jsonData)
//    {
//        if (!SteamAPI.IsSteamRunning())
//        {
//            Debug.LogError("Steam is not running.");
//            return;
//        }

//        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
//        Debug.Log($"Attempting to save file: {fileName} with size: {bytes.Length} bytes");

//        bool success = SteamRemoteStorage.FileWrite(fileName, bytes, bytes.Length);
//        if (success)
//        {
//            Debug.Log($"{fileName} saved successfully to Steam Cloud.");
//        }
//        else
//        {
//            Debug.LogError($"Failed to save {fileName} to Steam Cloud.");

//            파일 존재 여부 확인
//            bool fileExists = SteamRemoteStorage.FileExists(fileName);
//            Debug.Log($"FileExists check for {fileName}: {fileExists}");

//            파일 크기 확인
//            int fileSize = SteamRemoteStorage.GetFileSize(fileName);
//            Debug.LogError($"File size for {fileName} on Steam Cloud: {fileSize} bytes");

//            추가적인 디버깅 정보
//            EResult result = SteamRemoteStorage.GetFileWriteAsyncResult(fileName); // 존재하지 않는 메서드
//            Debug.LogError($"Last SteamRemoteStorage error: {result}");

//        }
//    }

//    Steam 클라우드에서 데이터 로드
//    public string LoadFromSteamCloud(string fileName)
//    {
//        if (SteamRemoteStorage.FileExists(fileName))
//        {
//            int fileSize = SteamRemoteStorage.GetFileSize(fileName);
//            byte[] bytes = new byte[fileSize];
//            SteamRemoteStorage.FileRead(fileName, bytes, fileSize);
//            string json = System.Text.Encoding.UTF8.GetString(bytes);
//            Debug.Log(fileName + " loaded successfully from Steam Cloud.");
//            return json;
//        }
//        else
//        {
//            Debug.LogWarning("No " + fileName + " found in Steam Cloud.");
//            return null;

//        }
//    }

//    public void PlayerSaveGameData()
//    {
//        string json = JsonUtility.ToJson(playerState, true);
//        File.WriteAllText(PlayerDataPath, json);
//        Debug.Log("Game data saved to " + PlayerDataPath);

//        Steam 클라우드에 저장
//        SaveToSteamCloud("playerState.json", json);
//    }

//    public void PrologueSaveGameData()
//    {
//        string json = JsonUtility.ToJson(prologue, true);
//        File.WriteAllText(QuestDataPath, json);
//        Debug.Log("Game data saved to " + QuestDataPath);

//        Steam 클라우드에 저장
//        SaveToSteamCloud("Quest.json", json);
//    }

//    public void TutorialSaveGameData()
//    {
//        string json = JsonUtility.ToJson(tutorial, true);
//        File.WriteAllText(tutorialDataPath, json);
//        Debug.Log("Game data saved to " + tutorialDataPath);

//        Steam 클라우드에 저장
//        SaveToSteamCloud("Tutorial.json", json);
//    }

//    public void MainSaveGameData()
//    {
//        string json = JsonUtility.ToJson(mainQuest, true);
//        File.WriteAllText(mainDataPath, json);
//        Debug.Log("Game data saved to " + mainDataPath);

//        Steam 클라우드에 저장
//        SaveToSteamCloud("MainQuest.json", json);
//    }

//    public void DailySaveGameData()
//    {
//        string json = JsonUtility.ToJson(dailyQuest, true);
//        File.WriteAllText(dailyDataPath, json);
//        Debug.Log("Game data saved to " + dailyDataPath);

//        Steam 클라우드에 저장
//        SaveToSteamCloud("dailyQuest.json", json);
//    }

//    public void PlayerLoadGameData()
//    {
//        string json = LoadFromSteamCloud("playerState.json");
//        if (json == null && File.Exists(PlayerDataPath))
//        {
//            json = File.ReadAllText(PlayerDataPath);
//            Debug.Log("Game data loaded from " + PlayerDataPath);
//        }
//        if (json != null)
//        {
//            JsonUtility.FromJsonOverwrite(json, playerState);
//            isDataLoaded = true;
//        }
//    }

//    private void PrologueLoadGameData()
//    {
//        string json = LoadFromSteamCloud("Quest.json");
//        if (json == null && File.Exists(QuestDataPath))
//        {
//            json = File.ReadAllText(QuestDataPath);
//            Debug.Log("Prologue data loaded from " + QuestDataPath);
//        }
//        if (json != null)
//        {
//            JsonUtility.FromJsonOverwrite(json, prologue);
//            isPrologued = true;
//        }
//    }

//    private void TutorialLoadGameData()
//    {
//        string json = LoadFromSteamCloud("Tutorial.json");
//        if (json == null && File.Exists(tutorialDataPath))
//        {
//            json = File.ReadAllText(tutorialDataPath);
//            Debug.Log("Tutorial data loaded from " + tutorialDataPath);
//        }
//        if (json != null)
//        {
//            JsonUtility.FromJsonOverwrite(json, tutorial);
//        }
//    }

//    private void MainLoadGameData()
//    {
//        string json = LoadFromSteamCloud("MainQuest.json");
//        if (json == null && File.Exists(mainDataPath))
//        {
//            json = File.ReadAllText(mainDataPath);
//            Debug.Log("Main data loaded from " + mainDataPath);
//        }
//        if (json != null)
//        {
//            JsonUtility.FromJsonOverwrite(json, mainQuest);
//        }
//    }

//    private void DailyLoadGameData()
//    {
//        string json = LoadFromSteamCloud("dailyQuest.json");
//        if (json == null && File.Exists(dailyDataPath))
//        {
//            json = File.ReadAllText(dailyDataPath);
//            Debug.Log("Daily data loaded from " + dailyDataPath);
//        }
//        if (json != null)
//        {
//            JsonUtility.FromJsonOverwrite(json, dailyQuest);
//        }
//    }

//    private void OnApplicationQuit()
//    {
//        PlayerSaveGameData();
//        PrologueSaveGameData();
//        TutorialSaveGameData();
//        MainSaveGameData();
//        DailySaveGameData();
//    }
//}





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
    public Item[] items;

    public bool isDataLoaded = false; // 데이터 로드 완료 상태 플래그
    public bool isPrologued = false;

    string PlayerDataPath;
    string QuestDataPath;
    string tutorialDataPath;
    string inventoryDataPath;
    
    

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
        inventoryDataPath = Path.Combine(Application.persistentDataPath, "Inventory.json");
        PlayerLoadGameData();
        PrologueLoadGameData();
        TutorialLoadGameData();
        //InventoryLoadGameData();
    }
    [System.Serializable]
    public class QuestScriptableArray
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
    //public void InventorySaveGameData()
    //{
    //    QuestScriptableArray itemsArray = new QuestScriptableArray { items = items };
    //    string json = JsonUtility.ToJson(itemsArray, true);
    //    File.WriteAllText(inventoryDataPath, json);
    //    Debug.Log("Inventory data saved to " + inventoryDataPath);
    //}

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
            isPrologued = true;
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
        //InventorySaveGameData();
    }
}

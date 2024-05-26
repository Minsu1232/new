using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{/// <summary>
/// money에 관련된 수치를 할당할 중앙 money 스크립트
/// </summary>
    public static MoneyManager Instance { get; private set; }
    public Money money;
    private string savePath;
    public Text coinText;
    public TextMeshProUGUI getCoinText;
    Coroutine coroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            money = ScriptableObject.CreateInstance<Money>(); // 초기화
            savePath = Path.Combine(Application.persistentDataPath, "money.json");
            money.OnMoneyChanged += HandleMoneyChanged; // 이벤트 구독
            LoadMoney();
        }
        else
        {
            Destroy(gameObject);
        }
        coroutine = StartCoroutine(GetCoinTextOff());
    }

    public void SaveMoney()
    {
        string json = JsonUtility.ToJson(money);
        File.WriteAllText(savePath, json);
        Debug.Log("Money saved to JSON: " + json);
    }

    public void LoadMoney()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, money);
            Debug.Log("Money loaded from JSON: " + json);
            Inventory.instance.moneyData = money;
        }
        else
        {
            Debug.Log("No money data to load.");
        }
    }
    private void HandleMoneyChanged(int newMoneyAmount, int changeAmount)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine); // 현재 실행 중인 코루틴 중지
            getCoinText.text = ""; // 텍스트 초기화
        }
        coroutine = StartCoroutine(GetCoinTextOff()); // 새 코루틴 시작
        Debug.Log("Money updated to: " + newMoneyAmount);
        coinText.text = newMoneyAmount.ToString(); // 총 금액
        getCoinText.gameObject.SetActive(true); 
        getCoinText.text = $"{changeAmount}전 획득 하였습니다."; // 획득한 금액
          
        
       
    }
    IEnumerator GetCoinTextOff()
    {
        yield return new WaitForSeconds(1f);
        getCoinText.gameObject.SetActive(false);
    }
    private void OnApplicationQuit()
    {
        SaveMoney();
    }

    private void OnDisable()
    {
        SaveMoney();
        money.OnMoneyChanged -= HandleMoneyChanged; // 이벤트 구독 해제
    }
}


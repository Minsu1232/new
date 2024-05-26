using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{/// <summary>
/// money�� ���õ� ��ġ�� �Ҵ��� �߾� money ��ũ��Ʈ
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
            money = ScriptableObject.CreateInstance<Money>(); // �ʱ�ȭ
            savePath = Path.Combine(Application.persistentDataPath, "money.json");
            money.OnMoneyChanged += HandleMoneyChanged; // �̺�Ʈ ����
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
            StopCoroutine(coroutine); // ���� ���� ���� �ڷ�ƾ ����
            getCoinText.text = ""; // �ؽ�Ʈ �ʱ�ȭ
        }
        coroutine = StartCoroutine(GetCoinTextOff()); // �� �ڷ�ƾ ����
        Debug.Log("Money updated to: " + newMoneyAmount);
        coinText.text = newMoneyAmount.ToString(); // �� �ݾ�
        getCoinText.gameObject.SetActive(true); 
        getCoinText.text = $"{changeAmount}�� ȹ�� �Ͽ����ϴ�."; // ȹ���� �ݾ�
          
        
       
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
        money.OnMoneyChanged -= HandleMoneyChanged; // �̺�Ʈ ���� ����
    }
}


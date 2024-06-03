using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class BowEnHancementSystem : MonoBehaviour
{
    public int baseEnhancementCost = 100; // 기본 강화 비용
    public int baseEnhacementItem = 1;
    public TextMeshProUGUI bowLevel;
    public TextMeshProUGUI bowLevelState;
    public TextMeshProUGUI bowDamage;
    public TextMeshProUGUI bowNextLevel;
    public TextMeshProUGUI successChance;
    public TextMeshProUGUI requireItem;
    public TextMeshProUGUI requireGold;
    public TextMeshProUGUI failorsucess;
    public Weapon playerBow;

    

  
    private void Start()
    {
        UpdateUI();
    }
    public bool EnhanceBow(Weapon bow)
    {
      
        // 강화 비용 계산 (기본 비용에 20%씩 증가)
        int cost = CalculateEnhancementCost(bow.enhancementLevel);
        int itemCost = CalculateEnhancementItem(bow.enhancementLevel);
        if (!Inventory.instance.HasGradeItems(itemCost))
        {
            failorsucess.text = "재료가 부족합니다.";
            StartCoroutine(textUI());
            Debug.Log("Not enough GradeItems to enhance the bow.");
            return false; // 인벤토리에 GradeItem이 없는 경우 강화 실패
        }
        // 플레이어가 충분한 자원을 가지고 있는지 확인 (가상의 MoneyManager 클래스 사용)
        if (MoneyManager.Instance.money.money >= cost )
        {
            MoneyManager.Instance.money.money -= cost; // 자원 차감
            Inventory.instance.UseGradeItems(itemCost);
            // 강화 성공 확률 계산
            int enhancementLevel = bow.enhancementLevel; // 현재 강화 레벨에서 시도하는 것
            float successChance = CalculateSuccessChance(enhancementLevel);

            // 랜덤 값을 이용해 성공 여부 결정
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= successChance)
            {
                bow.enhancementLevel++; // 강화 레벨 증가
                bow.enhancedAttackPower = CalculateEnhancedAttackPower(bow); // 강화된 공격력 계산
                Debug.Log($"Enhancement successful! New level: {bow.enhancementLevel}");
                failorsucess.text = "강화에 성공하셨습니다.";
                StartCoroutine(textUI());
                return true; // 강화 성공
            }
            else
            {
                Debug.Log("Enhancement failed.");
                failorsucess.text = "강화에 실패하셨습니다.";
                StartCoroutine(textUI());
                return false; // 강화 실패
            }
        }
        else
        {
            Debug.Log("Not enough resources to enhance the bow.");
            return false; // 강화 실패
        }
    }

    private float CalculateSuccessChance(int enhancementLevel)
    {
        // 각 강화 단계별 성공 확률 설정
        switch (enhancementLevel)
        {
            case 0: return 100f;
            case 1: return 90f;
            case 2: return 80f;
            case 3: return 70f;
            case 4: return 60f;
            case 5: return 50f;
            case 6: return 40f;
            case 7: return 30f;
            case 8: return 20f;
            case 9: return 10f;
            case 10: return 1f;
            default: return 0f;
        }
    }

    private int CalculateEnhancementCost(int enhancementLevel)
    {
        // 기본 강화 비용에 20%씩 증가시키는 로직
        return Mathf.RoundToInt(baseEnhancementCost * Mathf.Pow(1.2f, enhancementLevel));

               
    }
    private int CalculateEnhancementItem(int enhancementLevel)
    {
        // 기본 강화 재료 레벨에 맞게 증가시키는 로직
        return Mathf.RoundToInt(baseEnhacementItem * Mathf.Pow(1.5f , enhancementLevel));

    }
    

    private int CalculateEnhancedAttackPower(Weapon bow)
    {
        if (bow.enhancementLevel < 5)
        {
            // 강화 레벨이 5 미만인 경우 1씩 증가
            return bow.baseAttackPower + (bow.enhancementLevel * 1);
        }
        else
        {
            // 강화 레벨이 5 이상인 경우 2씩 증가
            return bow.baseAttackPower + (bow.enhancementLevel * 2);
        }
    }
    public void UpdateUI()
    {
        // 활의 현재 레벨 업데이트
        bowLevel.text = "예궁 + " + playerBow.enhancementLevel.ToString();
        bowLevelState.text = "예궁 + " + playerBow.enhancementLevel.ToString();
        // 다음 레벨 업데이트
        bowNextLevel.text = "예궁 " + (playerBow.enhancementLevel).ToString() + "단계 > " + (playerBow.enhancementLevel + 1).ToString() + "단계";

        // 성공 확률 업데이트
        successChance.text = "성공확률 : " + CalculateSuccessChance(playerBow.enhancementLevel) + "%";

        Item gradeItem = Inventory.instance.items.Find(item => item.itemName == "GradeItem");
        if (gradeItem != null)
        {
            requireItem.text = gradeItem.possess + " / " + CalculateEnhancementItem(playerBow.enhancementLevel).ToString();
        }
        else
        {
            requireItem.text = "0 / " + CalculateEnhancementItem(playerBow.enhancementLevel).ToString();
        }

        // 필요한 골드 업데이트
        requireGold.text = "재련 비용 : " + CalculateEnhancementCost(playerBow.enhancementLevel).ToString();
        bowDamage.text = "예궁 공격력 : " + (playerBow.baseAttackPower+ playerBow.enhancedAttackPower).ToString();
    }
    IEnumerator textUI()
    {
        failorsucess.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        failorsucess.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        UpdateUI();
    }
}

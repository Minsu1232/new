using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class BowEnHancementSystem : MonoBehaviour
{
    public int baseEnhancementCost = 100; // �⺻ ��ȭ ���
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
      
        // ��ȭ ��� ��� (�⺻ ��뿡 20%�� ����)
        int cost = CalculateEnhancementCost(bow.enhancementLevel);
        int itemCost = CalculateEnhancementItem(bow.enhancementLevel);
        if (!Inventory.instance.HasGradeItems(itemCost))
        {
            failorsucess.text = "��ᰡ �����մϴ�.";
            StartCoroutine(textUI());
            Debug.Log("Not enough GradeItems to enhance the bow.");
            return false; // �κ��丮�� GradeItem�� ���� ��� ��ȭ ����
        }
        // �÷��̾ ����� �ڿ��� ������ �ִ��� Ȯ�� (������ MoneyManager Ŭ���� ���)
        if (MoneyManager.Instance.money.money >= cost )
        {
            MoneyManager.Instance.money.money -= cost; // �ڿ� ����
            Inventory.instance.UseGradeItems(itemCost);
            // ��ȭ ���� Ȯ�� ���
            int enhancementLevel = bow.enhancementLevel; // ���� ��ȭ �������� �õ��ϴ� ��
            float successChance = CalculateSuccessChance(enhancementLevel);

            // ���� ���� �̿��� ���� ���� ����
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= successChance)
            {
                bow.enhancementLevel++; // ��ȭ ���� ����
                bow.enhancedAttackPower = CalculateEnhancedAttackPower(bow); // ��ȭ�� ���ݷ� ���
                Debug.Log($"Enhancement successful! New level: {bow.enhancementLevel}");
                failorsucess.text = "��ȭ�� �����ϼ̽��ϴ�.";
                StartCoroutine(textUI());
                return true; // ��ȭ ����
            }
            else
            {
                Debug.Log("Enhancement failed.");
                failorsucess.text = "��ȭ�� �����ϼ̽��ϴ�.";
                StartCoroutine(textUI());
                return false; // ��ȭ ����
            }
        }
        else
        {
            Debug.Log("Not enough resources to enhance the bow.");
            return false; // ��ȭ ����
        }
    }

    private float CalculateSuccessChance(int enhancementLevel)
    {
        // �� ��ȭ �ܰ躰 ���� Ȯ�� ����
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
        // �⺻ ��ȭ ��뿡 20%�� ������Ű�� ����
        return Mathf.RoundToInt(baseEnhancementCost * Mathf.Pow(1.2f, enhancementLevel));

               
    }
    private int CalculateEnhancementItem(int enhancementLevel)
    {
        // �⺻ ��ȭ ��� ������ �°� ������Ű�� ����
        return Mathf.RoundToInt(baseEnhacementItem * Mathf.Pow(1.5f , enhancementLevel));

    }
    

    private int CalculateEnhancedAttackPower(Weapon bow)
    {
        if (bow.enhancementLevel < 5)
        {
            // ��ȭ ������ 5 �̸��� ��� 1�� ����
            return bow.baseAttackPower + (bow.enhancementLevel * 1);
        }
        else
        {
            // ��ȭ ������ 5 �̻��� ��� 2�� ����
            return bow.baseAttackPower + (bow.enhancementLevel * 2);
        }
    }
    public void UpdateUI()
    {
        // Ȱ�� ���� ���� ������Ʈ
        bowLevel.text = "���� + " + playerBow.enhancementLevel.ToString();
        bowLevelState.text = "���� + " + playerBow.enhancementLevel.ToString();
        // ���� ���� ������Ʈ
        bowNextLevel.text = "���� " + (playerBow.enhancementLevel).ToString() + "�ܰ� > " + (playerBow.enhancementLevel + 1).ToString() + "�ܰ�";

        // ���� Ȯ�� ������Ʈ
        successChance.text = "����Ȯ�� : " + CalculateSuccessChance(playerBow.enhancementLevel) + "%";

        Item gradeItem = Inventory.instance.items.Find(item => item.itemName == "GradeItem");
        if (gradeItem != null)
        {
            requireItem.text = gradeItem.possess + " / " + CalculateEnhancementItem(playerBow.enhancementLevel).ToString();
        }
        else
        {
            requireItem.text = "0 / " + CalculateEnhancementItem(playerBow.enhancementLevel).ToString();
        }

        // �ʿ��� ��� ������Ʈ
        requireGold.text = "��� ��� : " + CalculateEnhancementCost(playerBow.enhancementLevel).ToString();
        bowDamage.text = "���� ���ݷ� : " + (playerBow.baseAttackPower+ playerBow.enhancedAttackPower).ToString();
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

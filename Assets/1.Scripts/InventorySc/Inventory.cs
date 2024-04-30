using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Item[] potions = new Item[2]; // ���ǽ�ũ���ͺ������Ʈ
    public Money moneyData;
    public int selectedSlot = 0;
    public Player player;
    public Image potionImage; // UI �̹��� ������Ʈ ����
    public Sprite hpPotionSprite; // HP ���� �̹���
    public Sprite mpPotionSprite; // MP ���� �̹���
    public Text potionRemain;
    public Text money;


    void Start()
    {
        selectedSlot = 0;
        UpdatePotionImage(); // �ʱ� �̹��� ����
        potions[0].possess = 99;// �׽�Ʈ������
        potions[1].possess = 99;



    }
    private void Update()
    {
        potionRemain.text = potions[selectedSlot].possess.ToString();
        money.text = moneyData.money.ToString();
    }

    public void SwitchSlot()
    {
        selectedSlot = (selectedSlot + 1) % 2; // ���� ��ȯ ����
        Debug.Log("Switched to slot " + (selectedSlot + 1));
        UpdatePotionImage(); // �̹��� ������Ʈ
    }

    public void UsePotion()
    {
        Item currentPotion = selectedSlot == 0 ? potions[selectedSlot] : potions[selectedSlot];
        if (potions[selectedSlot] != null && currentPotion.possess > 0)
        {
            potions[selectedSlot].Use(player); // ���õ� ���� ���
        }
        else
        {
            Debug.Log("No potion in slot " + (selectedSlot + 1));
        }
    }

    void UpdatePotionImage()
    {
        if (selectedSlot == 0)
        {
            potionImage.sprite = hpPotionSprite; // HP �̹��� ����
        }
        else
        {
            potionImage.sprite = mpPotionSprite; // MP �̹��� ����
        }
    }
}

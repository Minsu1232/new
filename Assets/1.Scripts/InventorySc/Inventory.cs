using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Item[] potions = new Item[2]; // 포션스크립터블오브젝트
    public Money moneyData;
    public int selectedSlot = 0;
    public Player player;
    public Image potionImage; // UI 이미지 컴포넌트 참조
    public Sprite hpPotionSprite; // HP 포션 이미지
    public Sprite mpPotionSprite; // MP 포션 이미지
    public Text potionRemain;
    public Text money;


    void Start()
    {
        selectedSlot = 0;
        UpdatePotionImage(); // 초기 이미지 설정
        potions[0].possess = 99;// 테스트를위해
        potions[1].possess = 99;



    }
    private void Update()
    {
        potionRemain.text = potions[selectedSlot].possess.ToString();
        money.text = moneyData.money.ToString();
    }

    public void SwitchSlot()
    {
        selectedSlot = (selectedSlot + 1) % 2; // 슬롯 전환 로직
        Debug.Log("Switched to slot " + (selectedSlot + 1));
        UpdatePotionImage(); // 이미지 업데이트
    }

    public void UsePotion()
    {
        Item currentPotion = selectedSlot == 0 ? potions[selectedSlot] : potions[selectedSlot];
        if (potions[selectedSlot] != null && currentPotion.possess > 0)
        {
            potions[selectedSlot].Use(player); // 선택된 포션 사용
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
            potionImage.sprite = hpPotionSprite; // HP 이미지 설정
        }
        else
        {
            potionImage.sprite = mpPotionSprite; // MP 이미지 설정
        }
    }
}

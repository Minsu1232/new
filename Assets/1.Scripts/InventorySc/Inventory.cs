using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;


    public Item[] potions = new Item[2]; // 포션스크립터블오브젝트
    public List<Item> items = new List<Item>();
    public Image[] inventorySlotImage;
    public Money moneyData;
    public int selectedSlot = 0;
    public Player player;
    public Image potionImage; // UI 이미지 컴포넌트 참조
    public Sprite hpPotionSprite; // HP 포션 이미지
    public Sprite mpPotionSprite; // MP 포션 이미지
    public Text potionRemain;
    public Text[] inventory;
    public Text money;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // 인스턴스를 할당
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 중복된 인스턴스 제거
        }
    }
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
    public void AddItem(Item item)
    {
        Item foundItem = items.Find(i => i.itemName == item.itemName);
       
        items.Add(item);
        item.possess ++;
        for (int i = 0; i < instance.inventorySlotImage.Length; i++)
        {

            if (instance.inventorySlotImage[i].sprite == null)
            {
                instance.inventory[i].text = item.possess.ToString(); // 슬릇창과 포션 갯수 연동
                if (instance.inventory[i].text.ToString() == "0") // 인벤토리 소지가 0일땐 text(false)
                {
                    instance.inventory[i].gameObject.SetActive(false);
                }
                else
                {
                    instance.inventory[i].gameObject.SetActive(true);
                }
                instance.inventorySlotImage[i].sprite = item.icon;
                break; // 슬롯에 이미지를 할당하고 나면 루프 종료
            }
            else if (instance.inventorySlotImage[i].sprite == item.icon)
            {
                instance.inventory[i].text = item.possess.ToString(); // 슬릇창과 포션 갯수 연동
                break; // 이미지가 이미 할당되어 있고, 같은 아이콘일 경우 루프 종료
            }
        }

        }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
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
    void UseCoinBundle()
    {
        
    }
   
}

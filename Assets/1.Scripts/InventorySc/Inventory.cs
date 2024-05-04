using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            potions[selectedSlot].Use(player, potions[selectedSlot].itemName); // 선택된 포션 사용
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
    public void UpdateUI(Item updatedItem)
    {
        // 각 슬롯을 순회하며 해당 아이템의 UI를 업데이트
        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            // 해당 슬롯의 아이템과 업데이트할 아이템이 일치하는 경우에만 UI를 업데이트
            if (inventorySlotImage[i].sprite == updatedItem.icon)
            {
                inventory[i].text = updatedItem.possess.ToString(); // 아이템 갯수 업데이트
                inventory[i].gameObject.SetActive(updatedItem.possess > 0); // 아이템 소지량에 따라 활성화/비활성화
                if (updatedItem.possess == 0)
                {
                    inventorySlotImage[i].sprite = null; // 아이템 소지량이 0이면 스프라이트 제거
                }
                break; // 해당 아이템을 찾았으므로 루프 종료
            }
        }
    }

}

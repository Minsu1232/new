using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    
    public Money playerMoney;    
    public Item[] itemsForSale;
    public Image[] itemImages; // 상점 UI에 9개의 이미지 슬롯
    public Text[] itemName;
    public Text[] itemPrice;
    Text remainPotion;
    

    void Start()
    {
        UpdateShopDisplay();
        
    }

    public void BuyItem(Item item)
    {
        if (playerMoney.money >= item.price)
        {
            playerMoney.money -= item.price; // 가격만큼 돈 감소
            //item.possess += 1; // 갯수 증가
            Debug.Log(item.name + " purchased for " + item.price + ". Remaining money: " + playerMoney.money);
            Inventory.instance.AddItem(item);  // 인벤토리에 아이템 추가 로직 추가 예정
            //for (int i = 0; i < Inventory.instance.inventorySlotImage.Length; i++)
            //{
               
            //    if (Inventory.instance.inventorySlotImage[i].sprite == null)
            //    {
            //        Inventory.instance.inventory[i].text = item.possess.ToString(); // 슬릇창과 포션 갯수 연동
            //        if (Inventory.instance.inventory[i].text.ToString() == "0") // 인벤토리 소지가 0일땐 text(false)
            //        {
            //            Inventory.instance.inventory[i].gameObject.SetActive(false);
            //        }
            //        else
            //        {
            //            Inventory.instance.inventory[i].gameObject.SetActive(true);
            //        }
            //        Inventory.instance.inventorySlotImage[i].sprite = item.icon;
            //        break; // 슬롯에 이미지를 할당하고 나면 루프 종료
            //    }
            //    else if (Inventory.instance.inventorySlotImage[i].sprite == item.icon)
            //    {
            //        Inventory.instance.inventory[i].text = item.possess.ToString(); // 슬릇창과 포션 갯수 연동
            //        break; // 이미지가 이미 할당되어 있고, 같은 아이콘일 경우 루프 종료
            //    }
               
                
            //}
        }
        else
        {
            Debug.Log("Not enough money to buy " + item.name);
        }
    }

    public void UpdateShopDisplay()
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (i < itemsForSale.Length)
            {
                itemName[i].text = itemsForSale[i].itemName; // 아이템 이름
                itemPrice[i].text = itemsForSale[i].price.ToString(); // 아이템 가격
                itemImages[i].sprite = itemsForSale[i].icon; // 아이템 이미지아이콘
                itemImages[i].gameObject.SetActive(true); // 판매 아이템 활성화
            }
            else
            {
                itemImages[i].gameObject.SetActive(false); // 비활성화
            }
        }
    }
}

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
    

    void Start()
    {
        UpdateShopDisplay();
        
    }

    public void BuyItem(Item item)
    {
        if (playerMoney.money >= item.price)
        {
            playerMoney.money -= item.price;
            Debug.Log(item.potionName + " purchased for " + item.price + ". Remaining money: " + playerMoney.money);
            // 인벤토리에 아이템 추가 로직 추가 예정
        }
        else
        {
            Debug.Log("Not enough money to buy " + item.potionName);
        }
    }

    public void UpdateShopDisplay()
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (i < itemsForSale.Length)
            {
                itemName[i].text = itemsForSale[i].potionName;
                itemPrice[i].text = itemsForSale[i].price.ToString();
                itemImages[i].sprite = itemsForSale[i].icon;
                itemImages[i].gameObject.SetActive(true); // 판매 아이템 활성화
            }
            else
            {
                itemImages[i].gameObject.SetActive(false); // 비활성화
            }
        }
    }
}

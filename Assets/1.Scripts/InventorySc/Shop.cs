using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    
    public Money playerMoney;    
    public Item[] itemsForSale;
    public Image[] itemImages; // ���� UI�� 9���� �̹��� ����
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
            playerMoney.money -= item.price; // ���ݸ�ŭ �� ����
            //item.possess += 1; // ���� ����
            Debug.Log(item.name + " purchased for " + item.price + ". Remaining money: " + playerMoney.money);
            Inventory.instance.AddItem(item);  // �κ��丮�� ������ �߰� ���� �߰� ����
            //for (int i = 0; i < Inventory.instance.inventorySlotImage.Length; i++)
            //{
               
            //    if (Inventory.instance.inventorySlotImage[i].sprite == null)
            //    {
            //        Inventory.instance.inventory[i].text = item.possess.ToString(); // ����â�� ���� ���� ����
            //        if (Inventory.instance.inventory[i].text.ToString() == "0") // �κ��丮 ������ 0�϶� text(false)
            //        {
            //            Inventory.instance.inventory[i].gameObject.SetActive(false);
            //        }
            //        else
            //        {
            //            Inventory.instance.inventory[i].gameObject.SetActive(true);
            //        }
            //        Inventory.instance.inventorySlotImage[i].sprite = item.icon;
            //        break; // ���Կ� �̹����� �Ҵ��ϰ� ���� ���� ����
            //    }
            //    else if (Inventory.instance.inventorySlotImage[i].sprite == item.icon)
            //    {
            //        Inventory.instance.inventory[i].text = item.possess.ToString(); // ����â�� ���� ���� ����
            //        break; // �̹����� �̹� �Ҵ�Ǿ� �ְ�, ���� �������� ��� ���� ����
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
                itemName[i].text = itemsForSale[i].itemName; // ������ �̸�
                itemPrice[i].text = itemsForSale[i].price.ToString(); // ������ ����
                itemImages[i].sprite = itemsForSale[i].icon; // ������ �̹���������
                itemImages[i].gameObject.SetActive(true); // �Ǹ� ������ Ȱ��ȭ
            }
            else
            {
                itemImages[i].gameObject.SetActive(false); // ��Ȱ��ȭ
            }
        }
    }
}

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
    

    void Start()
    {
        UpdateShopDisplay();
        
    }

    public void BuyItem(Item item)
    {
        if (playerMoney.money >= item.price)
        {
            playerMoney.money -= item.price; // ���ݸ�ŭ �� ����
            item.possess += 1; // ���� ����
            Debug.Log(item.potionName + " purchased for " + item.price + ". Remaining money: " + playerMoney.money);
            // �κ��丮�� ������ �߰� ���� �߰� ����
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
                itemName[i].text = itemsForSale[i].potionName; // ������ �̸�
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

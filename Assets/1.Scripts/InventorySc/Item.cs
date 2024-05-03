using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject, IItem
{
    public string itemName; // �̸�
    public string potionType; // Ÿ��
    public int effectAmount; // ȸ����
    public int possess; // ������
    public Sprite icon;
    public int price;
    

    public void Use(Player player) // Player ��ü�� �� �޼��带 ȣ���� �� ���޵˴ϴ�.
    {
        if(possess > 0)
        {
            if (potionType == "HP")
            {
                possess--;
                int newHealth = player.remainHealth + effectAmount;
                player.remainHealth = Mathf.Min(newHealth, player.initialHealth); // ������ �ִ� HP�� �ѱ�� ���� ����
                Debug.Log("HP Potion used. " + effectAmount + " Health restored.");
            }
            else if (potionType == "MP")
            {
                possess--;
                int newMp = player.mp + effectAmount;
                player.mp = Mathf.Min(newMp, player.maxMp); // ������ �ִ� HP�� �ѱ�� ���� ����
                Debug.Log("MP Potion used. " + effectAmount + " Mana restored.");
            }
           
        }       
    }
    
    public void MoneyBundleUse(Item item)
    {
        if(possess > 0)
        {
            if(itemName == "����������")
            {
                Money money = new Money();
                int baseRandom = Random.Range(1, 10); // 1���� 9���� ������ ���� ����
                int price = baseRandom * 100; // ������ ���� 100�� ���Ͽ� 100�� ������ ����
                money.money += price;
                possess--;
                Debug.Log(price+"ȹ��");
                
                
                
            }
        }
    }
  
}

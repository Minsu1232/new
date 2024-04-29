using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject, IItem
{
    public string potionName; // �̸�
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

                int newHealth = player.remainHealth + effectAmount;
                player.remainHealth = Mathf.Min(newHealth, player.initialHealth); // ������ �ִ� HP�� �ѱ�� ���� ����
                Debug.Log("HP Potion used. " + effectAmount + " Health restored.");
            }
            else if (potionType == "MP")
            {
                int newMp = player.mp + effectAmount;
                player.mp = Mathf.Min(newMp, player.maxMp); // ������ �ִ� HP�� �ѱ�� ���� ����
                Debug.Log("MP Potion used. " + effectAmount + " Mana restored.");
            }
            possess--;// ���� ����
        }
       
    }
    public void BuyItem()
    {

    }
}

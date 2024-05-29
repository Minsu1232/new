using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject, IItem
{
    public string itemName; // ������ �����̸�
    public string itemRealName; // ������ ǥ��� �̸�
    public string potionType; // Ÿ��
    public int effectAmount; // ȸ����
    public int possess; // ������
    public Sprite icon;
    public Money money;
    public int price;
    public Item saveItems;
    public string description;




    public void Use(Player player, string name) // Player ��ü�� �� �޼��带 ȣ���� �� ���޵˴ϴ�. ������ ��� ȿ��
                                                // ���� ������ �߰� �ſ� ����
    {
        if (possess <= 0)
        {
            return;
        }

        player = player.GetComponent<Player>();
        possess--;

        switch (name)
        {
            case "HP":
                int newHealth = player.remainHealth + effectAmount;
                player.remainHealth = Mathf.Min(newHealth, player.playerState.health); // ������ �ִ� HP�� �ѱ�� ���� ����
                player.hp.text = $"{player.remainHealth}/{player.playerState.health}";
                player.hpBar.fillAmount = (float)player.remainHealth / player.playerState.health;
                Debug.Log("HP Potion used. " + effectAmount + " Health restored.");
                break;

            case "MP":
                int newMp = player.mp + effectAmount;
                player.mp = Mathf.Min(newMp, player.playerState.mp); // ������ �ִ� HP�� �ѱ�� ���� ����
                player.mana.text = $"{player.mp}/{player.playerState.mp}";
                player.mpBar.fillAmount = (float)player.mp / player.playerState.mp;
                Debug.Log("MP Potion used. " + effectAmount + " Mana restored.");
                break;

            case "CoinBundle":
                int baseRandom = Random.Range(1, 10); // 1���� 9���� ������ ���� ����
                int price = baseRandom * 100; // ������ ���� 100�� ���Ͽ� 100�� ������ ����
                MoneyManager.Instance.money.money += price;
                Debug.Log(price + "ȹ��");
                break;

            case "GradeItem":
            case "GradeItem2":
                break;

        }




    }
}

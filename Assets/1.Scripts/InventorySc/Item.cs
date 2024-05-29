using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject, IItem
{
    public string itemName; // 아이템 고유이름
    public string itemRealName; // 아이템 표기용 이름
    public string potionType; // 타입
    public int effectAmount; // 회복량
    public int possess; // 소지량
    public Sprite icon;
    public Money money;
    public int price;
    public Item saveItems;
    public string description;




    public void Use(Player player, string name) // Player 객체는 이 메서드를 호출할 때 전달됩니다. 아이템 사용 효과
                                                // 현재 아이템 추가 매우 용이
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
                player.remainHealth = Mathf.Min(newHealth, player.playerState.health); // 포션이 최대 HP를 넘기게 차지 않음
                player.hp.text = $"{player.remainHealth}/{player.playerState.health}";
                player.hpBar.fillAmount = (float)player.remainHealth / player.playerState.health;
                Debug.Log("HP Potion used. " + effectAmount + " Health restored.");
                break;

            case "MP":
                int newMp = player.mp + effectAmount;
                player.mp = Mathf.Min(newMp, player.playerState.mp); // 포션이 최대 HP를 넘기게 차지 않음
                player.mana.text = $"{player.mp}/{player.playerState.mp}";
                player.mpBar.fillAmount = (float)player.mp / player.playerState.mp;
                Debug.Log("MP Potion used. " + effectAmount + " Mana restored.");
                break;

            case "CoinBundle":
                int baseRandom = Random.Range(1, 10); // 1부터 9까지 랜덤한 정수 생성
                int price = baseRandom * 100; // 생성된 수에 100을 곱하여 100의 단위로 만듦
                MoneyManager.Instance.money.money += price;
                Debug.Log(price + "획득");
                break;

            case "GradeItem":
            case "GradeItem2":
                break;

        }




    }
}

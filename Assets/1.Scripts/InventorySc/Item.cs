using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject, IItem
{
    public string itemName; // 이름
    public string potionType; // 타입
    public int effectAmount; // 회복량
    public int possess; // 소지량
    public Sprite icon;
    public int price;
    

    public void Use(Player player) // Player 객체는 이 메서드를 호출할 때 전달됩니다.
    {
        if(possess > 0)
        {
            if (potionType == "HP")
            {
                possess--;
                int newHealth = player.remainHealth + effectAmount;
                player.remainHealth = Mathf.Min(newHealth, player.initialHealth); // 포션이 최대 HP를 넘기게 차지 않음
                Debug.Log("HP Potion used. " + effectAmount + " Health restored.");
            }
            else if (potionType == "MP")
            {
                possess--;
                int newMp = player.mp + effectAmount;
                player.mp = Mathf.Min(newMp, player.maxMp); // 포션이 최대 HP를 넘기게 차지 않음
                Debug.Log("MP Potion used. " + effectAmount + " Mana restored.");
            }
           
        }       
    }
    
    public void MoneyBundleUse(Item item)
    {
        if(possess > 0)
        {
            if(itemName == "동전보따리")
            {
                Money money = new Money();
                int baseRandom = Random.Range(1, 10); // 1부터 9까지 랜덤한 정수 생성
                int price = baseRandom * 100; // 생성된 수에 100을 곱하여 100의 단위로 만듦
                money.money += price;
                possess--;
                Debug.Log(price+"획득");
                
                
                
            }
        }
    }
  
}

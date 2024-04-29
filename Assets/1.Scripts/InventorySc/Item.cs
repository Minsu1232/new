using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject, IItem
{
    public string potionName; // 이름
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

                int newHealth = player.remainHealth + effectAmount;
                player.remainHealth = Mathf.Min(newHealth, player.initialHealth); // 포션이 최대 HP를 넘기게 차지 않음
                Debug.Log("HP Potion used. " + effectAmount + " Health restored.");
            }
            else if (potionType == "MP")
            {
                int newMp = player.mp + effectAmount;
                player.mp = Mathf.Min(newMp, player.maxMp); // 포션이 최대 HP를 넘기게 차지 않음
                Debug.Log("MP Potion used. " + effectAmount + " Mana restored.");
            }
            possess--;// 사용시 감소
        }
       
    }
    public void BuyItem()
    {

    }
}

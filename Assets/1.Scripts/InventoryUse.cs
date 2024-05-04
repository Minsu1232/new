
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryUse : MonoBehaviour, IPointerClickHandler
{
    public Player player;
    public int itemCount;
    public Item item;
    public Image[] items;

    public void OnPointerClick(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        item = Inventory.instance.items.Find(i => i.itemName == image.sprite.name); // 스프라이트와 아이템 이름을 통해 아이템 참조

        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            Debug.Log("Using item: " + image.sprite.name);
            item.Use(player, item.itemName); // 아이템 사용
            itemCount = item.possess; // 아이템 수량 업데이트

            Inventory.instance.UpdateUI(item); // 아이템에 대한 UI만 업데이트

            if (item.possess == 0) // 아이템의 수량이 0이 되면 아이템 제거
            {
                Inventory.instance.RemoveItem(item);
            }
        }

    }
    int FindSpriteIndexByName(string spriteName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].sprite != null && items[i].sprite.name == spriteName)
            {
                return i;
            }
        }
        return -1; // 스프라이트를 찾지 못했을 경우 -1 반환
    }



    void UseItem(string name)
    {
       
        item.Use(player,name);
        
    }


   
}


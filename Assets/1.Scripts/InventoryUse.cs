
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
        item = Inventory.instance.items.Find(i => i.itemName == image.sprite.name); // ��������Ʈ�� ������ �̸��� ���� ������ ����

        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            Debug.Log("Using item: " + image.sprite.name);
            item.Use(player, item.itemName); // ������ ���
            itemCount = item.possess; // ������ ���� ������Ʈ

            Inventory.instance.UpdateUI(item); // �����ۿ� ���� UI�� ������Ʈ

            if (item.possess == 0) // �������� ������ 0�� �Ǹ� ������ ����
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
        return -1; // ��������Ʈ�� ã�� ������ ��� -1 ��ȯ
    }



    void UseItem(string name)
    {
       
        item.Use(player,name);
        
    }


   
}


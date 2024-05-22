
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;


public class InventoryUse : MonoBehaviour, IPointerClickHandler
{
    public Player player;
    public int itemCount;
    public Item item;
    public Image[] items;

    string inventoryDataPath;
    private void OnEnable()
    {
        
    }
    private void Start()
    {

        
    }

  
 
    public void OnPointerClick(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();        
        item = Inventory.instance.items.Find(i => i.itemName == image.sprite.name); // ��������Ʈ�� ������ �̸��� ���� ������ ����
        Debug.Log("������ũ��");
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            Debug.Log("Using item: " + image.sprite.name);
            item.Use(player, item.itemName); // ������ ���
            itemCount = item.possess; // ������ ���� ������Ʈ
            if(item.itemName == "CoinBundle")
            {
                Inventory.instance.money.text = item.money.money.ToString(); // ���̽����� �Ҵ� �� �Ӵ��� �ؽ�Ʈ �ʱ�ȭ
            }
            
            Inventory.instance.UpdateUI(item); // �����ۿ� ���� UI�� ������Ʈ
            
            if (item.possess == 0) // �������� ������ 0�� �Ǹ� ������ ����
            {
                Inventory.instance.RemoveItem(item);               

            }
        
        }
        //Inventory.instance.SaveInventory();
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


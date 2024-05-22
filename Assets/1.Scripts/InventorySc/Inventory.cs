using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class Inventory : MonoBehaviour
{
  
    public static Inventory instance;


    public Item[] potions = new Item[2]; // ���ǽ�ũ���ͺ������Ʈ
    public List<Item> items = new List<Item>(); 
    public Image[] inventorySlotImage;
    public Money moneyData;
    public int selectedSlot = 0;
    public Player player;
    public Image potionImage; // UI �̹��� ������Ʈ ����
    public Sprite hpPotionSprite; // HP ���� �̹���
    public Sprite mpPotionSprite; // MP ���� �̹���
    public Text potionRemain;
    public Text[] inventory;
    public Text money;
    [System.Serializable]
    public class ItemData
    {
        public string saveItem;
        public string itemName;
        public int possess;
        public string spriteName;
        public int imageIndex;
        public int itemsIndex;
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        LoadInventory();
        //LoadInventoryFromJson();
        selectedSlot = 0;
        UpdatePotionImage(); // �ʱ� �̹��� ����
        potionRemain.text = potions[selectedSlot].possess.ToString();


    }
    private void Update()
    {
           
        money.text = moneyData.money.ToString();
    }
    [System.Serializable]
    public class InventoryData
    {
        public string[] potionIDs;
        public List<string> itemIDs;
        public int remainPotion;
        public int remainPotion_;
        public int remainPotion1;
        public int remainPotion1_;
    }
  
    public class Serialization<T>
    {
        public T Data;
    }
  
    public void SaveInventory()
    {
        List<ItemData> dataToSave = new List<ItemData>();
        foreach (var item in items)
        {
            int index = Array.FindIndex(inventorySlotImage, slot => slot.sprite == item.icon);
            int itemIndex = items.FindIndex(items => items.itemName == item.itemName);

            dataToSave.Add(new ItemData()
            {

                saveItem = item.saveItems.name,
                itemName = item.itemName,
                possess = item.possess,
                spriteName = item.icon.name,
                imageIndex = index,
                itemsIndex = itemIndex
                
            });
        }
      
        string json = JsonUtility.ToJson(new Serialization<List<ItemData>>() { Data = dataToSave }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
        Debug.Log("Inventory saved successfully at " + Application.persistentDataPath + "/inventory.json");
    

    }
    public List<Item> LoadInventory()
    {
        string filePath = Application.persistentDataPath + "/inventory.json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            Serialization<List<ItemData>> savedData = JsonUtility.FromJson<Serialization<List<ItemData>>>(json);
            List<Item> loadedItems = new List<Item>();
            foreach (var itemData in savedData.Data)
            {
               
                Item item = ScriptableObject.CreateInstance<Item>();
                
                item.itemName = itemData.itemName;
                item.possess = itemData.possess;
                item.icon = Resources.Load<Sprite>(itemData.spriteName);

                int index = Array.FindIndex(potions, i => i.itemName  == item.itemName);
                loadedItems.Add(item);
                items.Add(item);
                //potions[index].possess = item.possess;
                Debug.Log("Inventory Loaded successfully at " + Application.persistentDataPath + "/inventory.json");
                // UI ������Ʈ ����
                if (itemData.imageIndex != -1 && itemData.imageIndex < inventorySlotImage.Length)
                {
                    inventorySlotImage[itemData.imageIndex].sprite = item.icon;
                    inventory[itemData.imageIndex].text = item.possess.ToString();
                    inventory[itemData.imageIndex].gameObject.SetActive(true);
                    UpdateUI(item);
                }
              


                UpdateUI(item);  // �ε�� �����ۿ� ���� UI ������Ʈ
            }
            return loadedItems;
        }
        return new List<Item>(); // ������ ���ٸ� �� ����Ʈ ��ȯ
    }

    public void AddItem(Item item)
    {
        Item foundItem = items.Find(i => i.itemName == item.itemName);

        if (foundItem != null)
        {
            Debug.Log(foundItem);
            // �������� �̹� �����ϸ� ������ ����
            foundItem.possess++;
            UpdateUI(item);
            // �ش� �������� ������ ã�� ������ ������Ʈ
            for (int i = 0; i < inventorySlotImage.Length; i++)
            {//��ĭ�� ��������� �̹� �κ��丮�� �ִ� �������̸� ����
                
                if (inventorySlotImage[i].sprite == foundItem.icon)
                {
                    inventory[i].text = foundItem.possess.ToString();
                    UpdateUI(foundItem);
                    break;
                }
            }
        }
        else
        {
            // �� �������� �κ��丮�� �߰�
            items.Add(item);
            item.possess++;
            UpdateUI(item);
            for (int i = 0; i < inventorySlotImage.Length; i++)
            {
                if (inventorySlotImage[i].sprite == null)
                {
                    inventorySlotImage[i].sprite = item.icon;
                    inventory[i].text = item.possess.ToString();
                    inventory[i].gameObject.SetActive(true);
                   
                    break;
                }
            }
        }
        SaveInventory();
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        UpdateUI(item);
    }

    public void SwitchSlot()
    {
        selectedSlot = (selectedSlot + 1) % 2; // ���� ��ȯ ����
        
        UpdatePotionImage(); // �̹��� ������Ʈ
        UpdateUI(potions[selectedSlot]);
    }

    public void UsePotion()
    {
        Item currentPotion = selectedSlot == 0 ? potions[selectedSlot] : potions[selectedSlot];
        if (potions[selectedSlot] != null && currentPotion.possess > 0)
        {
            potions[selectedSlot].Use(player, potions[selectedSlot].itemName); // ���õ� ���� ���
            UpdateUI(currentPotion);

        }
        else
        {
            Debug.Log("No potion in slot " + (selectedSlot + 1));
        }
    }

    void UpdatePotionImage()
    {
        if (selectedSlot == 0)
        {
            potionImage.sprite = hpPotionSprite; // HP �̹��� ����
        }
        else
        {
            potionImage.sprite = mpPotionSprite; // MP �̹��� ����
        }
    }
    public void UpdateUI(Item updatedItem)
    {
        // �� ������ ��ȸ�ϸ� �ش� �������� UI�� ������Ʈ
        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            // �ش� ������ �����۰� ������Ʈ�� �������� ��ġ�ϴ� ��쿡�� UI�� ������Ʈ
            if (inventorySlotImage[i].sprite == updatedItem.icon)
            {
                inventory[i].text = updatedItem.possess.ToString(); // ������ ���� ������Ʈ
                potionRemain.text = potions[selectedSlot].possess.ToString();
                inventory[i].gameObject.SetActive(updatedItem.possess > 0); // ������ �������� ���� Ȱ��ȭ/��Ȱ��ȭ
                if (updatedItem.possess == 0)
                {
                    inventorySlotImage[i].sprite = null; // ������ �������� 0�̸� ��������Ʈ ����
                }
                break; // �ش� �������� ã�����Ƿ� ���� ����
            }
        }
    }
    private void OnApplicationQuit()
    {
        
    }

}

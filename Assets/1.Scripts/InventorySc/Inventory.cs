using Steamworks;
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

    public GameObject[] healingEffect;
    [System.Serializable]
    public class ItemData
    {

        public string itemName;
        public int possess;
        public string spriteName;
        public int imageIndex;
        public int itemsIndex;
        public int moneyAmount; // Money ��ũ���ͺ��� ȭ�� ���� ����
        public int effectAmount;
        public int price;
        public string realName;
        public string description;
        public string potionType;

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
        //potionRemain.text = potions[selectedSlot].possess.ToString();
        money.text = moneyData.money.ToString();


    }
    void Update()
    {
        QclickPotion();
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
            if (item.money == null || item.icon == null)
            {
                //Debug.LogError("saveItems or icon is null");
                //continue; // Skip this item or handle it differently
            }

            int index = Array.FindIndex(inventorySlotImage, slot => slot.sprite == item.icon);
            int itemIndex = items.FindIndex(items => items.itemName == item.itemName);
            //if (index == -1 || itemIndex == -1)
            //{
            //    Debug.LogError("Invalid index found");
            //    continue; // Skip this item or handle it differently
            //}
            dataToSave.Add(new ItemData()
            {

                // �߰��Ǵ� �������� ������ ���⿡ �߰�
                itemName = item.itemName != null ? item.itemName : "Unknown Item", // �⺻������ "Unknown Item" ����
                possess = item.possess, // possess�� int�̹Ƿ� null üũ�� �ʿ� �����ϴ�.
                spriteName = item.icon != null ? item.icon.name : "DefaultIcon", // �⺻������ "DefaultIcon" ����
                imageIndex = (item.icon != null && index != -1) ? index : -1, // icon�� null�� �ƴϰ�, ��ȿ�� index�� �ִ� ��쿡�� �Ҵ�
                itemsIndex = itemIndex != -1 ? itemIndex : -1, // ��ȿ�� itemsIndex�� �Ҵ�
                moneyAmount = item.money != null ? item.money.money : 0, // �⺻������ 0 ����
                effectAmount = item.effectAmount,
                price = item.price,
                realName = item.itemRealName,
                description = item.description,
                potionType = item.potionType




            }); ;
            //if (item.itemName == "HP" )
            //{
            //    potions[0].possess = item.possess; 
            //}
            //else if(item.itemName == "MP")
            //{
            //    potions[1].possess = item.possess;
            //}
        }

        string json = JsonUtility.ToJson(new Serialization<List<ItemData>>() { Data = dataToSave }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
        Debug.Log("Inventory saved successfully at " + Application.persistentDataPath + "/inventory.json");
        // ���� Ŭ���忡 ����
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        //bool success = SteamRemoteStorage.FileWrite("inventory.json", bytes, bytes.Length);
        //if (success)
        //{
        //    Debug.Log("Inventory saved successfully to Steam Cloud.");
        //}
        //else
        //{
        //    Debug.LogError("Failed to save inventory to Steam Cloud.");
        //}

    }
    //public List<Item> LoadInventory()
    //{
    //    if (SteamRemoteStorage.FileExists("inventory.json"))
    //    {
    //        int fileSize = SteamRemoteStorage.GetFileSize("inventory.json");
    //        byte[] bytes = new byte[fileSize];
    //        SteamRemoteStorage.FileRead("inventory.json", bytes, fileSize);
    //        string json = System.Text.Encoding.UTF8.GetString(bytes);

    //        Serialization<List<ItemData>> savedData = JsonUtility.FromJson<Serialization<List<ItemData>>>(json);
    //        List<Item> loadedItems = new List<Item>();
    //        foreach (var itemData in savedData.Data)
    //        {
    //            Item item = ScriptableObject.CreateInstance<Item>();
    //            item.itemName = itemData.itemName;
    //            item.possess = itemData.possess;
    //            item.icon = Resources.Load<Sprite>(itemData.spriteName);
    //            item.effectAmount = itemData.effectAmount;
    //            item.price = itemData.price;
    //            item.itemRealName = itemData.realName;
    //            item.description = itemData.description;
    //            item.potionType = itemData.potionType;

    //            if (item.itemName == "HP")
    //            {
    //                potions[0] = item;
    //            }
    //            if (item.itemName == "MP")
    //            {
    //                potions[1] = item;
    //            }

    //            loadedItems.Add(item);
    //            items.Add(item);
    //            if (item.possess == 0)
    //            {
    //                RemoveItem(item); // 0�� �Ǹ� ����Ʈ���� ����
    //            }

    //            if (itemData.imageIndex != -1 && itemData.imageIndex < inventorySlotImage.Length)
    //            {
    //                inventorySlotImage[itemData.imageIndex].sprite = item.icon;
    //                inventory[itemData.imageIndex].text = item.possess.ToString();
    //                inventory[itemData.imageIndex].gameObject.SetActive(true);
    //                UpdateUI(item);
    //            }

    //            UpdateUI(item);
    //        }
    //        Debug.Log("Inventory loaded successfully from Steam Cloud.");
    //        return loadedItems;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("No inventory file found in Steam Cloud.");
    //        return new List<Item>(); // ������ ���ٸ� �� ����Ʈ ��ȯ
    //    }
    //}
    // 20240602 ���̽�����
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
                // �߰��Ǵ� �������� ������ ���⿡ �߰�
                Item item = ScriptableObject.CreateInstance<Item>();

                item.itemName = itemData.itemName;
                item.possess = itemData.possess;
                item.icon = Resources.Load<Sprite>(itemData.spriteName);
                item.effectAmount = itemData.effectAmount;
                item.price = itemData.price;
                item.itemRealName = itemData.realName;
                item.description = itemData.description;
                item.potionType = itemData.potionType;

                if (item.itemName == "HP")
                {
                    potions[0] = item;

                }
                if (item.itemName == "MP")
                {
                    potions[1] = item;
                }
                //if (item.itemName == "CoinBundle")
                //{
                //    Money money = ScriptableObject.CreateInstance<Money>();
                //    item.money = money;
                //    money.money = itemData.moneyAmount; // ����� ȭ�� �������� �ʱ�ȭ

                //}
                if (item.itemName == "HP" || item.itemName == "MP")
                {
                    int index = Array.FindIndex(potions, i => i.itemName == item.itemName);
                    item.possess = potions[index].possess;
                    Money money = ScriptableObject.CreateInstance<Money>();
                    item.money = money;
                    item.price = itemData.price;
                    //money.money = itemData.moneyAmount; // ���ι����� �ӴϽ�ũ���ͺ� �Ҵ��� ���� �Բ� �Ҵ�
                }


                loadedItems.Add(item);
                items.Add(item);
                if (item.possess == 0)
                {
                    RemoveItem(item); // 0�̵Ǹ� ����Ʈ���� ����
                }
                Debug.Log("Inventory Loaded successfully at " + Application.persistentDataPath + "/inventory.json");
                // UI ������Ʈ ����
                if (itemData.imageIndex != -1 && itemData.imageIndex < inventorySlotImage.Length)
                {
                    inventorySlotImage[itemData.imageIndex].sprite = item.icon; // ����� ����Ʈ�� ������ �κ��丮�� �Ҵ�
                    inventory[itemData.imageIndex].text = item.possess.ToString(); // ���� �Ҵ�
                    inventory[itemData.imageIndex].gameObject.SetActive(true); // 0�̻��̴� �ؽ�Ʈ Ŵ
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
            // ���� �迭 ������Ʈ
            if (foundItem.itemName == "HP")
            {
                potions[0] = foundItem;
            }
            else if (foundItem.itemName == "MP")
            {
                potions[1] = foundItem;
            }
            Debug.Log(foundItem);
            // �������� �̹� �����ϸ� ������ ����
            //if(foundItem.itemName == "HP")
            //{
            //    potions[0].possess = foundItem.possess;
            //}
            //else if(foundItem.itemName == "MP")
            //{
            //    potions[1].possess = foundItem.possess;
            //}
            foundItem.possess++;
            UpdateUI(foundItem);
            // �ش� �������� ������ ã�� ������ ������Ʈ
            for (int i = 0; i < inventorySlotImage.Length; i++)
            {//��ĭ�� ��������� �̹� �κ��丮�� �ִ� �������̸� ����

                if (inventorySlotImage[i].sprite == foundItem.icon)
                {
                    inventory[i].text = foundItem.possess.ToString();

                    break;
                }
            }
        }
        else
        {// potions�� ���ο� items����Ʈ�� �Ҵ��� ��ũ���ͺ� ������Ʈ ����
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.itemName = item.itemName;
            newItem.icon = item.icon;
            newItem.effectAmount = item.effectAmount;
            newItem.money = item.money;
            newItem.possess = 0;
            newItem.description = item.description;
            newItem.itemRealName = item.itemRealName;
            newItem.potionType = item.potionType;
            // �迭 ���� �� ���Ӱ� �߰��Ǹ� ���ǽ��� ���� �ʱ⶧���� ���Ҵ�
            // ���� �迭 ������Ʈ
            if (newItem.itemName == "HP")
            {
                potions[0] = newItem;
            }
            else if (newItem.itemName == "MP")
            {
                potions[1] = newItem;
            }
            // �� �������� �κ��丮�� �߰�
            items.Add(newItem);
            newItem.possess++;
            UpdateUI(newItem);
            for (int i = 0; i < inventorySlotImage.Length; i++)
            {
                if (inventorySlotImage[i].sprite == null)
                {
                    inventorySlotImage[i].sprite = newItem.icon;
                    inventory[i].text = newItem.possess.ToString();
                    inventory[i].gameObject.SetActive(true);

                    break;
                }
            }
        }

    }
    public bool HasGradeItems(int requiredItems) // �ҷ������
    {
        Item gradeItem = items.Find(item => item.itemName == "GradeItem");
        return gradeItem != null && gradeItem.possess >= requiredItems;
    }

    public void UseGradeItems(int requiredItems) // ����
    { 
        Item gradeItem = items.Find(item => item.itemName == "GradeItem");
        if (gradeItem != null && gradeItem.possess >= requiredItems)
        {
            gradeItem.possess -= requiredItems;
            if (gradeItem.possess == 0)
            {
                items.Remove(gradeItem);
            }
            Debug.Log($"GradeItem used. Remaining: {gradeItem.possess}");
        }
        else
        {
            Debug.Log("Not enough GradeItems available.");
        }
    }
    void QclickPotion()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchSlot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UsePotion();
            // ������ ���� ��ƼŬȿ��
            if (selectedSlot == 0)
            {
                if (potions[0].possess > 0)
                {
                    healingEffect[1].SetActive(true);
                }

            }
            else
            {
                if (potions[1].possess > 0)
                {
                    healingEffect[0].SetActive(true);
                }
            }





        }
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

            Item index = items.Find(i => i.itemName == currentPotion.itemName);
            potions[selectedSlot].possess = index.possess;

            if (index.possess == 0)
            {
                RemoveItem(index);
            }
            UpdateUI(index);
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
        money.text = moneyData.money.ToString();
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
    }

}

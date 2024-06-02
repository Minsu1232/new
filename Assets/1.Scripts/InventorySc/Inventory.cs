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


    public Item[] potions = new Item[2]; // 포션스크립터블오브젝트
    public List<Item> items = new List<Item>();
    public Image[] inventorySlotImage;
    public Money moneyData;
    public int selectedSlot = 0;
    public Player player;
    public Image potionImage; // UI 이미지 컴포넌트 참조
    public Sprite hpPotionSprite; // HP 포션 이미지
    public Sprite mpPotionSprite; // MP 포션 이미지
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
        public int moneyAmount; // Money 스크립터블의 화폐 수량 저장
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
        UpdatePotionImage(); // 초기 이미지 설정
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

                // 추가되는 아이템의 내용은 여기에 추가
                itemName = item.itemName != null ? item.itemName : "Unknown Item", // 기본값으로 "Unknown Item" 설정
                possess = item.possess, // possess는 int이므로 null 체크가 필요 없습니다.
                spriteName = item.icon != null ? item.icon.name : "DefaultIcon", // 기본값으로 "DefaultIcon" 설정
                imageIndex = (item.icon != null && index != -1) ? index : -1, // icon이 null이 아니고, 유효한 index가 있는 경우에만 할당
                itemsIndex = itemIndex != -1 ? itemIndex : -1, // 유효한 itemsIndex만 할당
                moneyAmount = item.money != null ? item.money.money : 0, // 기본값으로 0 설정
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
        // 스팀 클라우드에 저장
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
    //                RemoveItem(item); // 0이 되면 리스트에서 삭제
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
    //        return new List<Item>(); // 파일이 없다면 빈 리스트 반환
    //    }
    //}
    // 20240602 제이슨저장
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
                // 추가되는 아이템의 내용은 여기에 추가
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
                //    money.money = itemData.moneyAmount; // 저장된 화폐 수량으로 초기화

                //}
                if (item.itemName == "HP" || item.itemName == "MP")
                {
                    int index = Array.FindIndex(potions, i => i.itemName == item.itemName);
                    item.possess = potions[index].possess;
                    Money money = ScriptableObject.CreateInstance<Money>();
                    item.money = money;
                    item.price = itemData.price;
                    //money.money = itemData.moneyAmount; // 코인번들의 머니스크립터블 할당을 위해 함께 할당
                }


                loadedItems.Add(item);
                items.Add(item);
                if (item.possess == 0)
                {
                    RemoveItem(item); // 0이되면 리스트에서 삭제
                }
                Debug.Log("Inventory Loaded successfully at " + Application.persistentDataPath + "/inventory.json");
                // UI 업데이트 로직
                if (itemData.imageIndex != -1 && itemData.imageIndex < inventorySlotImage.Length)
                {
                    inventorySlotImage[itemData.imageIndex].sprite = item.icon; // 저장된 리스트의 아이을 인벤토리에 할당
                    inventory[itemData.imageIndex].text = item.possess.ToString(); // 갯수 할당
                    inventory[itemData.imageIndex].gameObject.SetActive(true); // 0이상이니 텍스트 킴
                    UpdateUI(item);
                }



                UpdateUI(item);  // 로드된 아이템에 대해 UI 업데이트
            }
            return loadedItems;
        }
        return new List<Item>(); // 파일이 없다면 빈 리스트 반환
    }

    public void AddItem(Item item)
    {
        Item foundItem = items.Find(i => i.itemName == item.itemName);

        if (foundItem != null)
        {
            // 포션 배열 업데이트
            if (foundItem.itemName == "HP")
            {
                potions[0] = foundItem;
            }
            else if (foundItem.itemName == "MP")
            {
                potions[1] = foundItem;
            }
            Debug.Log(foundItem);
            // 아이템이 이미 존재하면 수량만 증가
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
            // 해당 아이템의 슬롯을 찾아 정보를 업데이트
            for (int i = 0; i < inventorySlotImage.Length; i++)
            {//앞칸이 비어있지만 이미 인벤토리에 있는 아이템이면 누적

                if (inventorySlotImage[i].sprite == foundItem.icon)
                {
                    inventory[i].text = foundItem.possess.ToString();

                    break;
                }
            }
        }
        else
        {// potions와 새로운 items리스트에 할당할 스크립터블 오브젝트 생성
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.itemName = item.itemName;
            newItem.icon = item.icon;
            newItem.effectAmount = item.effectAmount;
            newItem.money = item.money;
            newItem.possess = 0;
            newItem.description = item.description;
            newItem.itemRealName = item.itemRealName;
            newItem.potionType = item.potionType;
            // 배열 삭제 후 새롭게 추가되면 포션스와 맞지 않기때문에 재할당
            // 포션 배열 업데이트
            if (newItem.itemName == "HP")
            {
                potions[0] = newItem;
            }
            else if (newItem.itemName == "MP")
            {
                potions[1] = newItem;
            }
            // 새 아이템을 인벤토리에 추가
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
    public bool HasGradeItems(int requiredItems) // 불러오기용
    {
        Item gradeItem = items.Find(item => item.itemName == "GradeItem");
        return gradeItem != null && gradeItem.possess >= requiredItems;
    }

    public void UseGradeItems(int requiredItems) // 사용용
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
            // 슬릇에 따른 파티클효과
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
        selectedSlot = (selectedSlot + 1) % 2; // 슬롯 전환 로직        
        UpdatePotionImage(); // 이미지 업데이트
        UpdateUI(potions[selectedSlot]);

    }

    public void UsePotion()
    {
        Item currentPotion = selectedSlot == 0 ? potions[selectedSlot] : potions[selectedSlot];
        if (potions[selectedSlot] != null && currentPotion.possess > 0)
        {
            potions[selectedSlot].Use(player, potions[selectedSlot].itemName); // 선택된 포션 사용           

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
            potionImage.sprite = hpPotionSprite; // HP 이미지 설정
        }
        else
        {
            potionImage.sprite = mpPotionSprite; // MP 이미지 설정
        }
    }
    public void UpdateUI(Item updatedItem)
    {
        // 각 슬롯을 순회하며 해당 아이템의 UI를 업데이트
        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            // 해당 슬롯의 아이템과 업데이트할 아이템이 일치하는 경우에만 UI를 업데이트
            if (inventorySlotImage[i].sprite == updatedItem.icon)
            {
                inventory[i].text = updatedItem.possess.ToString(); // 아이템 갯수 업데이트
                potionRemain.text = potions[selectedSlot].possess.ToString();
                inventory[i].gameObject.SetActive(updatedItem.possess > 0); // 아이템 소지량에 따라 활성화/비활성화
                if (updatedItem.possess == 0)
                {
                    inventorySlotImage[i].sprite = null; // 아이템 소지량이 0이면 스프라이트 제거
                }
                break; // 해당 아이템을 찾았으므로 루프 종료
            }
        }
        money.text = moneyData.money.ToString();
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
    }

}

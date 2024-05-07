using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        selectedSlot = 0;
        UpdatePotionImage(); // �ʱ� �̹��� ����
        potions[0].possess = 99;// �׽�Ʈ������
        potions[1].possess = 99;



    }
    private void Update()
    {
        potionRemain.text = potions[selectedSlot].possess.ToString();
        money.text = moneyData.money.ToString();
    }
    public void AddItem(Item item)
    {
        Item foundItem = items.Find(i => i.itemName == item.itemName);

        if (foundItem != null)
        {
            // �������� �̹� �����ϸ� ������ ����
            foundItem.possess++;
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
        {
            // �� �������� �κ��丮�� �߰�
            items.Add(item);
            item.possess++;
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
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void SwitchSlot()
    {
        selectedSlot = (selectedSlot + 1) % 2; // ���� ��ȯ ����
        Debug.Log("Switched to slot " + (selectedSlot + 1));
        UpdatePotionImage(); // �̹��� ������Ʈ
    }

    public void UsePotion()
    {
        Item currentPotion = selectedSlot == 0 ? potions[selectedSlot] : potions[selectedSlot];
        if (potions[selectedSlot] != null && currentPotion.possess > 0)
        {
            potions[selectedSlot].Use(player, potions[selectedSlot].itemName); // ���õ� ���� ���
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
                inventory[i].gameObject.SetActive(updatedItem.possess > 0); // ������ �������� ���� Ȱ��ȭ/��Ȱ��ȭ
                if (updatedItem.possess == 0)
                {
                    inventorySlotImage[i].sprite = null; // ������ �������� 0�̸� ��������Ʈ ����
                }
                break; // �ش� �������� ã�����Ƿ� ���� ����
            }
        }
    }

}

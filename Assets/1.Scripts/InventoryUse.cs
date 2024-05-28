
using JetBrains.Annotations;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;


public class InventoryUse : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Player player;
    public int itemCount;
    public Item item;
    public Image[] items;
    public Money playerMoney;
    public Text moneyText;

    public GameObject tooltipPanel;  // ���� �г� ��ü
    public TextMeshProUGUI descriptionText;     // ���� �ؽ�Ʈ
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI itemNameText;
    public Image itemImage;

    string inventoryDataPath;
    private void OnEnable()
    {

        //// �̺�Ʈ�� �޼��� ����
        //playerMoney.OnMoneyChanged += UpdateMoneyDisplay;
    }
    private void Start()
    {

        
    }
    private void Update()
    {
        if (Input.mousePosition != null && item != null)
        {
            tooltipPanel.transform.position = Input.mousePosition + new Vector3(150, 200, 0); // ���콺 ��ġ�� ���� ���� ��ġ ����
        }
        
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        
        Debug.Log("������ũ��");
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            
            item.Use(player, item.itemName); // ������ ���
            itemCount = item.possess; // ������ ���� ������Ʈ
            if(item.itemName == "CoinBundle")
            {
               
            }
            
            Inventory.instance.UpdateUI(item); // �����ۿ� ���� UI�� ������Ʈ
            
            if (item.possess == 0) // �������� ������ 0�� �Ǹ� ������ ����
            {
                Inventory.instance.RemoveItem(item);               

            }
        
        }
       
        //Inventory.instance.SaveInventory();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺�� �������� �ִ� ĭ�� �ø���

        // �̹��� ������Ʈ�� �Ҵ� ��
        Image image = GetComponent<Image>();
        // �κ��丮�� items ����Ʈ���� image ������ �̸��� ���� obj�� ����Ʈ���� �̸��� ã�� �� item�� �Ҵ�
        item = Inventory.instance.items.Find(i => i.itemName == image.sprite.name); // ��������Ʈ�� ������ �̸��� ���� ������ ����
        if (item != null)
        {
            ShowTooltip(); // ���콺 ���� �� ���� ǥ��
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip(); // ���콺�� �����ۿ��� ��� �� ���� ����
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

    public void ShowTooltip()
    {
        // ���� ����
        tooltipPanel.transform.position =  new Vector3(50f, 50f, 0);
        descriptionText.text = item.description;
        itemImage.sprite = item.icon;
        itemNameText.text = item.itemRealName;
        typeText.text = item.potionType;
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    void UseItem(string name)
    {
       
        item.Use(player,name);
        
    }
    //private void UpdateMoneyDisplay(int newMoneyAmount)
    //{
    //    Debug.Log("Money updated to: " + newMoneyAmount);
    //    // UI ������Ʈ ����, ��: moneyDisplayText.text = $"Money: {newMoneyAmount}";
    //    //moneyText.text = newMoneyAmount.ToString();
    //}
    //private void OnDisable()
    //{
    //    // �̺�Ʈ ���� ����
    //    playerMoney.OnMoneyChanged -= UpdateMoneyDisplay;
    //}
    //public void ShowTooltip()
    //{
       
    //    descriptionText.text = item.description;
    //    itemImage.sprite = item.icon;
    //    itemNameText.text = item.itemRealName;
    //    typeText.text = item.potionType;
       
    //    tooltipPanel.SetActive(true); // ���� Ȱ��ȭ
        
        
    //}

    //public void HideTooltip()
    //{
    //    tooltipPanel.SetActive(false);  // �г� ��Ȱ��ȭ
    //}


}


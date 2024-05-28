
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

    public GameObject tooltipPanel;  // 설명 패널 객체
    public TextMeshProUGUI descriptionText;     // 설명 텍스트
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI itemNameText;
    public Image itemImage;

    string inventoryDataPath;
    private void OnEnable()
    {

        //// 이벤트에 메서드 구독
        //playerMoney.OnMoneyChanged += UpdateMoneyDisplay;
    }
    private void Start()
    {

        
    }
    private void Update()
    {
        if (Input.mousePosition != null && item != null)
        {
            tooltipPanel.transform.position = Input.mousePosition + new Vector3(150, 200, 0); // 마우스 위치에 따라 툴팁 위치 조정
        }
        
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        
        Debug.Log("아이템크릭");
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            
            item.Use(player, item.itemName); // 아이템 사용
            itemCount = item.possess; // 아이템 수량 업데이트
            if(item.itemName == "CoinBundle")
            {
               
            }
            
            Inventory.instance.UpdateUI(item); // 아이템에 대한 UI만 업데이트
            
            if (item.possess == 0) // 아이템의 수량이 0이 되면 아이템 제거
            {
                Inventory.instance.RemoveItem(item);               

            }
        
        }
       
        //Inventory.instance.SaveInventory();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스를 아이템이 있는 칸에 올리면

        // 이미지 컴포넌트를 할당 후
        Image image = GetComponent<Image>();
        // 인벤토리의 items 리스트에서 image 변수의 이름과 같은 obj를 리스트에서 이름을 찾은 후 item에 할당
        item = Inventory.instance.items.Find(i => i.itemName == image.sprite.name); // 스프라이트와 아이템 이름을 통해 아이템 참조
        if (item != null)
        {
            ShowTooltip(); // 마우스 오버 시 툴팁 표시
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip(); // 마우스가 아이템에서 벗어날 때 툴팁 숨김
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

    public void ShowTooltip()
    {
        // 툴팁 설정
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
    //    // UI 업데이트 로직, 예: moneyDisplayText.text = $"Money: {newMoneyAmount}";
    //    //moneyText.text = newMoneyAmount.ToString();
    //}
    //private void OnDisable()
    //{
    //    // 이벤트 구독 해제
    //    playerMoney.OnMoneyChanged -= UpdateMoneyDisplay;
    //}
    //public void ShowTooltip()
    //{
       
    //    descriptionText.text = item.description;
    //    itemImage.sprite = item.icon;
    //    itemNameText.text = item.itemRealName;
    //    typeText.text = item.potionType;
       
    //    tooltipPanel.SetActive(true); // 툴팁 활성화
        
        
    //}

    //public void HideTooltip()
    //{
    //    tooltipPanel.SetActive(false);  // 패널 비활성화
    //}


}


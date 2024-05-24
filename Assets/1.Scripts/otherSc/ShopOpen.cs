using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpen : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject shop;
    
    // Start is called before the first frame update
    void Start()
    { 

    }
    // 마우스 커서가 오브젝트 위로 올라가면 윤곽선을 활성화합니다.
 
    // 마우스 커서가 오브젝트에서 벗어나면 윤곽선을 비활성화합니다.
   
    void OnMouseDown()
    {
        if (!shopMenu.activeSelf && !shop.activeSelf)
        {
            shopMenu.SetActive(true);
            GameManager.Instance.isShop = true;
        }
       
     
    }
    public void ShopOpening()
    {
        if (!shop.activeSelf)
        {
            shop.SetActive(true);
            shopMenu.SetActive(false);
        }
        
    }
    public void ShopMenuClose()
    {
        if (shopMenu.activeSelf)
        {
            shopMenu.SetActive(false);
        }
    }
}

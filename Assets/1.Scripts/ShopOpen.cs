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

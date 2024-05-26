using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShopOpen : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject shop;
    
    // Start is called before the first frame update
    void Start()
    { 

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopMenu.SetActive(false);
            shop.SetActive(false);

        }
    }
    // ���콺 Ŀ���� ������Ʈ ���� �ö󰡸� �������� Ȱ��ȭ�մϴ�.

    // ���콺 Ŀ���� ������Ʈ���� ����� �������� ��Ȱ��ȭ�մϴ�.

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

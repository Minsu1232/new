using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BowManager : MonoBehaviour
{
    public BowEnHancementSystem enhancementSystem;
   
    public Weapon playerBow;
    public GameObject panel;

   


    private void Start()
    {
        
    }
    // ��ȭ ��ư Ŭ�� �� ȣ��� �޼���
    public void EnhanceBow()
    {
        bool success = enhancementSystem.EnhanceBow(playerBow);
        enhancementSystem.UpdateUI(); // UI ������Ʈ
        panel.SetActive(false);
        if (success)
        {
            Debug.Log("Bow enhanced successfully!");
        }
        else
        {
            Debug.Log("Bow enhancement failed.");
        }
    }
   public void PanelOff()
    {
        panel.SetActive(false);
    }
}

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
    // 강화 버튼 클릭 시 호출될 메서드
    public void EnhanceBow()
    {
        bool success = enhancementSystem.EnhanceBow(playerBow);
        enhancementSystem.UpdateUI(); // UI 업데이트
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

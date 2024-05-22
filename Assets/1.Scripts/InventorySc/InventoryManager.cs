using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   
    public GameObject[] healingEffect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
             Inventory.instance.SwitchSlot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            // 슬릇에 따른 파티클효과
            if (Inventory.instance.selectedSlot == 0)
            {
                if (Inventory.instance.potionRemain.text != "0")
                {
                    healingEffect[0].SetActive(true);
                }
                
            }
            else
            {
                if (Inventory.instance.potionRemain.text != "0")
                {
                    healingEffect[0].SetActive(true);
                }
            }



            Inventory.instance.UsePotion();
        }
    }
}

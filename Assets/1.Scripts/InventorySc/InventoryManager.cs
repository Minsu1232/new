using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    public GameObject[] healingEffect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory.SwitchSlot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            // ������ ���� ��ƼŬȿ��
            if (inventory.selectedSlot == 0)
            {
                healingEffect[0].SetActive(true);
            }
            else
            {
                healingEffect[1].SetActive(true);
            }



            inventory.UsePotion();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory.SwitchSlot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.UsePotion();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public bool AddItem(Item_SO newItem, int amount)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (newItem.stackAble && inventoryItems[i].itemSo == newItem)
            {
                inventoryItems[i].amount += amount;
                return true;
            }

            if (inventoryItems[i].itemSo != null) continue;
            inventoryItems[i].itemSo = newItem;
            inventoryItems[i].amount = amount;
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class InventoryItem
{
    public Item_SO itemSo;
    public int amount;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    /// <summary>
    /// 刷新格子中的图标和数量
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            //改变每个slotHolders下的ItemUI的索引，使其能与InventoryData_SO中的inventoryItems一一对应上
            slotHolders[i].itemUI.index = i; 
            slotHolders[i].UpdateItem();
        }
    }
}

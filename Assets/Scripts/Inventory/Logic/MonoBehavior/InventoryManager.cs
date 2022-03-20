using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventor Data")] 
    public InventoryData_SO inventoryDataSo;

    [Header("Container")] 
    public ContainerUI inventoryUI;

    private void Start()
    {
        inventoryUI.RefreshUI();
    }
}

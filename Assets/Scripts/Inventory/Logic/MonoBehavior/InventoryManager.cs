using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [System.Serializable]
    public class DragData
    {
        public SlotHolder originalHolder; //拖拽的物体原本的SlotHolder
        public RectTransform originalTransform; //拖拽的物体的原本的Parent
    }
    
    [Header("Inventor Data")] 
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;
    
    [Header("Containers")] 
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData dragData;
    
    private void Start()
    {
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    #region 检查鼠标位于哪个背包UI之上

    public bool CheckInventoryUI(Vector3 position)
    {
        foreach (var t in inventoryUI.slotHolders)
        {
            var rect = t.transform as RectTransform;
            //判断该position是否处于RectTransform之中
            if (RectTransformUtility.RectangleContainsScreenPoint(rect,position))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CheckActionUI(Vector3 position)
    {
        foreach (var t in actionUI.slotHolders)
        {
            var rect = t.transform as RectTransform;
            //判断该position是否处于RectTransform之中
            if (RectTransformUtility.RectangleContainsScreenPoint(rect,position))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CheckEquipmentUI(Vector3 position)
    {
        foreach (var t in equipmentUI.slotHolders)
        {
            var rect = t.transform as RectTransform;
            //判断该position是否处于RectTransform之中
            if (RectTransformUtility.RectangleContainsScreenPoint(rect,position))
            {
                return true;
            }
        }

        return false;
    }
    
    #endregion
}

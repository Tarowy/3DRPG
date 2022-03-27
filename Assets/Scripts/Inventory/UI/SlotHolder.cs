using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum SlotType
{
    BAG,
    WEAPON,
    ARMOR,
    ACTION
}

public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                //如果该格子是背包里的，那么就将单例的背包数据传到里面去
                itemUI.bag = InventoryManager.Instance.inventoryData;
                Debug.Log("BAG:"+itemUI.bag);
                break;
            case SlotType.ARMOR:
                itemUI.bag = InventoryManager.Instance.equipmentData;
                Debug.Log("ARMOR:"+itemUI.bag);
                break;
            case SlotType.WEAPON:
                itemUI.bag = InventoryManager.Instance.equipmentData;
                Debug.Log("WEAPON:"+itemUI.bag);
                //武器槽位不为空则修改人物攻击数据
                if (itemUI.bag.inventoryItems[itemUI.index].itemSo != null)
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.bag.inventoryItems[itemUI.index].itemSo);
                }
                //武器槽位为空则还原人物原始攻击数据
                else
                {
                    GameManager.Instance.playerStats.UnEquipmentWeapon();
                }

                break;
            case SlotType.ACTION:
                itemUI.bag = InventoryManager.Instance.actionData;
                Debug.Log("ACTION:"+itemUI.bag);
                break;
        }

        //拿到InventoryData_SO中对应索引的InventoryItem数据
        var inventoryItem = itemUI.bag.inventoryItems[itemUI.index];
        itemUI.SetUpItemUI(inventoryItem.itemSo, inventoryItem.amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //如果点击的次数是2的倍数则使用该物品
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public bool UseItem()
    {
        if (!itemUI.GetInventoryItem().itemSo)
        {
            return false;
        }

        if (itemUI.GetInventoryItem().itemSo.itemType == ItemType.Usable && itemUI.GetInventoryItem().amount > 0)
        {
            //回血是否成功
            if (GameManager.Instance.playerStats.ApplyHealth(itemUI.GetInventoryItem().itemSo.usableItemSo.healthPoint))
            {
                itemUI.GetInventoryItem().amount--;
                UpdateItem();
                return true;
            }

            return false;
        }

        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Slot:" + itemUI.GetInventoryItem().itemSo);
        if (itemUI.GetInventoryItem().itemSo)
        {
            InventoryManager.Instance.itemToolTip.GetComponent<ItemToolTip>()
                .SetupToolTip(itemUI.GetInventoryItem().itemSo);
            InventoryManager.Instance.itemToolTip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemToolTip.SetActive(false);
    }

    public void OnDisable()
    {
        //在自己被Disable的时候也将信息UI隐藏
        InventoryManager.Instance.itemToolTip.SetActive(false);
    }
}
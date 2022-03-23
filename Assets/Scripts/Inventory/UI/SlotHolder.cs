using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
    BAG,
    WEAPON,
    ARMOR,
    ACTION
}

public class SlotHolder : MonoBehaviour
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
                break;
            case SlotType.ARMOR:
                itemUI.bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.WEAPON:
                itemUI.bag = InventoryManager.Instance.equipmentData;
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
                break;
        }

        //拿到InventoryData_SO中对应索引的InventoryItem数据
        var inventoryItem = itemUI.bag.inventoryItems[itemUI.index];
        itemUI.SetUpItemUI(inventoryItem.itemSo, inventoryItem.amount);
    }
}
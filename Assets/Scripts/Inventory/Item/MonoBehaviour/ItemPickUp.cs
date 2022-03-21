using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item_SO isoItemSo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (InventoryManager.Instance.inventoryData.AddItem(isoItemSo,isoItemSo.itemAmount))
            {
                InventoryManager.Instance.inventoryUI.RefreshUI();
                //销毁
                Destroy(gameObject);
            }
            
            // other.GetComponent<CharacterStats>().EquipWeapon(isoItemSo);
        }
    }
}

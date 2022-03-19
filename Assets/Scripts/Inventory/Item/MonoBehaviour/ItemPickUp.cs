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
            if (InventoryManager.Instance.inventoryDataSo.AddItem(isoItemSo,isoItemSo.itemAmount))
            {
                //销毁
                Destroy(gameObject);
            }
            
            // other.GetComponent<CharacterStats>().EquipWeapon(isoItemSo);
        }
    }
}

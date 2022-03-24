using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode keyCode;
    public SlotHolder currentHolder;

    private void Update()
    {
        //如果按下对应按键且当前栏位有物品则使用物品
        if (Input.GetKeyDown(keyCode) && currentHolder.itemUI.GetInventoryItem().itemSo)
        {
            currentHolder.UseItem();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image image;
    public Text amount;

    private RectTransform _rectTransform;
    
    public InventoryData_SO bag { set; get; }
    public int index { set; get; } = -1;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetUpItemUI(Item_SO itemSo, int itemAmount)
    {
        //如果该物品数量为0，则需要删除对应背包中的数据
        if (itemAmount == 0)
        {
            bag.inventoryItems[index].itemSo = null;
            image.gameObject.SetActive(false);
            return;
        }

        if (itemSo is null)
        {
            image.gameObject.SetActive(false);
            return;
        }

        image.sprite = itemSo.icon;
        amount.text = itemAmount.ToString();
        image.gameObject.SetActive(true);
    }

    public InventoryItem GetInventoryItem()
    {
        return bag.inventoryItems[index];
    }
}
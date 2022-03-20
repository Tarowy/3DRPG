using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image image;
    public Text amount;
    
    public InventoryData_SO bag { set; get; }
    public int index { set; get; } = -1;

    public void SetUpItemUI(Item_SO itemSo, int itemAmount)
    {
        if (itemSo is null)
        {
            image.gameObject.SetActive(false);
            return;
        }
        
        image.sprite = itemSo.icon;
        amount.text = itemAmount.ToString();
        image.gameObject.SetActive(true);
    }
}

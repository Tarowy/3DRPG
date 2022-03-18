using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Usable,
    Weapon,
    Armor
}
[CreateAssetMenu(fileName = "Item",menuName = "Inventory/Item Data")]
public class Item_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int itemAmount;
    [TextArea] public string description = "";
    public bool stackAble;

    public GameObject weaponPrefab;
}

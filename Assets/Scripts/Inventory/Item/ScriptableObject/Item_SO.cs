using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UsableItem")] 
    public UsableItem_SO usableItemSo;

    [Header("WeaponItem")]
    public GameObject weaponPrefab;
    public AttackData_SO attackDataSo;
}

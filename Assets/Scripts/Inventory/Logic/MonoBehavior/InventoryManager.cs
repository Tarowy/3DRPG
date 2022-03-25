using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Stats")] 
    public Text healthText;
    public Text attackText;
    public Text defenceText;
    public Text criticalText;

    [Header("Components")] 
    public GameObject bagPanel;
    public GameObject equipPanel;

    [Header("ItemToolTip")] 
    public GameObject itemToolTip;

    private bool _isOpen;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    private void Update()
    {
        SwitchInventory();

        var playerStats = GameManager.Instance.playerStats;
        UpdateStats(playerStats.CurrentHealth, playerStats.attackDataSo.minDamage, playerStats.attackDataSo.maxDamage,
            playerStats.CurrentDefence, playerStats.attackDataSo.criticalChance);
    }

    private void SwitchInventory()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;
        _isOpen = !_isOpen;
        equipPanel.SetActive(_isOpen);
        bagPanel.SetActive(_isOpen);
    }

    private void UpdateStats(int health, int attackMin, int attackMax, int defence, float critical)
    {
        healthText.text = health.ToString();
        attackText.text = attackMin + " ~ " + attackMax;
        defenceText.text = defence.ToString();
        criticalText.text = (critical * 100).ToString() + "%";
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

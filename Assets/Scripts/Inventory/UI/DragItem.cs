using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private ItemUI _currentItemUI;
    private SlotHolder _currentHolder;
    private SlotHolder _targetHolder;

    private void Awake()
    {
        _currentItemUI = GetComponent<ItemUI>();
        _currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //拖拽时将本物体的父对象的SlotHolder数据和Transform保存到DragData中
        InventoryManager.Instance.dragData = new InventoryManager.DragData();
        InventoryManager.Instance.dragData.originalHolder = _currentHolder;
        InventoryManager.Instance.dragData.originalTransform = (RectTransform) _currentHolder.transform;
        //设置其父物体，是否保持local坐标系的所有状态还是改变成word坐标系
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //拖拽物体随着鼠标的位置移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //判断鼠标的位置是不是处于UI之上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //判断鼠标是否位于SlotHolder格子之上
            if (InventoryManager.Instance.CheckActionUI(eventData.position) ||
                InventoryManager.Instance.CheckEquipmentUI(eventData.position) ||
                InventoryManager.Instance.CheckInventoryUI(eventData.position))
            {
                Debug.Log(eventData.pointerEnter.gameObject);
                //判断鼠标的停留的位置有没有SlotHolder：
                //  如果SlotHolder里的ItemUI没有物品则鼠标就是在SlotHolder之上；
                //  如果SlotHolder里的ItemUI有物品则鼠标就是在ItemUI里的Image之上这时就需要获取父对象的SlotHolder
                _targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>()
                    ? eventData.pointerEnter.gameObject.GetComponent<SlotHolder>()
                    : eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();

                switch (_targetHolder.slotType)
                {
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    case SlotType.WEAPON:
                        break;
                    case SlotType.ARMOR:
                        break;
                    case SlotType.ACTION:
                        break;
                }
                _currentHolder.UpdateItem();
                _targetHolder.UpdateItem();
            }
        }

        transform.SetParent(InventoryManager.Instance.dragData.originalTransform);
        var rectTransform = transform as RectTransform;
        rectTransform.offsetMax = -Vector2.one * 5;
        rectTransform.offsetMin = Vector2.one * 5;
    }

    /// <summary>
    /// 根据拖拽的物体的ItemUI的索引和目标位置的ItemUI索引交换InventoryData中List对应索引的值
    /// </summary>
    private void SwapItem()
    {
        //拖拽的ItemUI的索引所对应的InventoryItem
        var tempInventoryItem = _currentHolder.itemUI.bag.inventoryItems[_currentHolder.itemUI.index];
        //目标位置的ItemUI的索引所对应的InventoryItem
        var targetInventoryItem = _targetHolder.itemUI.bag.inventoryItems[_targetHolder.itemUI.index];

        var isSameItem = tempInventoryItem.itemSo == targetInventoryItem.itemSo;
        //根据索引交换对应InventoryItem的数据
        //如果是同一个物品且可堆叠就直接叠加
        if (isSameItem && targetInventoryItem.itemSo.stackAble)
        {
            targetInventoryItem.amount += tempInventoryItem.amount;
            tempInventoryItem.itemSo = null;
            tempInventoryItem.amount = 0;
        }
        else //如果不是同一个则直接换位置
        {
            _currentHolder.itemUI.bag.inventoryItems[_currentHolder.itemUI.index] = targetInventoryItem;
            _targetHolder.itemUI.bag.inventoryItems[_targetHolder.itemUI.index] = tempInventoryItem;
        }
    }
}

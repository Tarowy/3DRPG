using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        
    }
}

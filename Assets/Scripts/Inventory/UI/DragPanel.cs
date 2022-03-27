using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    private RectTransform _rectTransform;
    public Canvas canvas;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// 根据鼠标的位移量改变锚点的位移量
    /// 由于Canvas分辨率和屏幕分辨率的原因鼠标的移动量得除以画布的缩放
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    /// <summary>
    /// 拖拽物品完毕需要将其在父物体下的索引排到前面防止被其他UI覆盖
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.SetSiblingIndex(2);
    }
}

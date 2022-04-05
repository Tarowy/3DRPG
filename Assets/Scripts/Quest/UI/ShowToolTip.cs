using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private ItemUI _itemUI;

    private void Awake()
    {
        _itemUI = GetComponent<ItemUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestUI.Instance.itemToolTip.gameObject.SetActive(true);
        QuestUI.Instance.itemToolTip.SetupToolTip(_itemUI.currentItemDataSo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.itemToolTip.gameObject.SetActive(false);
    }
}

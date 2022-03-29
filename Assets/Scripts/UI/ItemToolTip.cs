using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public Text nameText;
    public Text infoText;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        //刚开始的时候update还未为执行就直接active了UI，
        //导致UI生成在了鼠标位置遮盖SlotHolder从而触发鼠标离开SlotHolder的接口导致闪烁
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void SetupToolTip(Item_SO itemSo)
    {
        nameText.text = itemSo.itemName;
        infoText.text = itemSo.description;
    }

    private void UpdatePosition()
    {
        var mousePos = Input.mousePosition;

        var corners = new Vector3[4];
        //获取该UI四个角落的坐标，左下角为0,0点
        _rectTransform.GetWorldCorners(corners);

        //右边的角坐标-左边的角坐标就是宽度
        var width = corners[3].x - corners[0].x;
        //上边的角坐标-下边的角坐标就是高度
        var height = corners[1].y - corners[0].y;

        //如果鼠标距离屏幕底端的高度小于UI的高度就需要在鼠标上面显示
        if (mousePos.y < height)
        {
            _rectTransform.position = mousePos + Vector3.up * height * 0.8f;
        }
        //默认是在鼠标左边显示
        else if (Screen.width - mousePos.x > width)
        {
            _rectTransform.position = mousePos + Vector3.right * width * 0.7f;
        }
        //如果鼠标与屏幕左边的距离小于UI的宽度们就需要在鼠标左边显示
        else
        {
            _rectTransform.position = mousePos + Vector3.left * width * 0.7f;
        }
    }
}

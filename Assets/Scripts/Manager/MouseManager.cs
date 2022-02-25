using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [System.Serializable] //该类不是继承于monoBehaviour所以需要序列化才能在Unity中显示出来
// public class EventVector3 : UnityEvent<Vector3> { } //类似于委托，可以委托Unity的事件
public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance; //单例模式
    public event Action<Vector3> _onMouseClicked; //鼠标点击的委托
    public Texture2D point, doorway, attack, target, arrow;

    private RaycastHit _hitInfo;

    private void Awake()
    {
        if (Instance is null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    private void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    private void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray,out _hitInfo)) //Ray碰上了东西则是true，out返回碰撞到的Object的信息
        {
            //切换鼠标贴图
            switch (_hitInfo.collider.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto); //hotspot是鼠标中心点的便宜，原点为左上角
                    break;
            }
        }
    }

    private void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && _hitInfo.collider != null)
        {
            if (_hitInfo.collider.tag.Equals("Ground"))
            {
                _onMouseClicked?.Invoke(_hitInfo.point); //如果ray碰到了地面则将点传到Unity的事件委托里
            }
        }
    }
}
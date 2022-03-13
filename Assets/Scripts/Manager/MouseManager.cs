using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [System.Serializable] //该类不是继承于monoBehaviour所以需要序列化才能在Unity中显示出来
// public class EventVector3 : UnityEvent<Vector3> { } //类似于委托，可以委托Unity的事件
public class MouseManager : Singleton<MouseManager>
{
    public event Action<Vector3> ONMouseClicked; //鼠标点击的委托
    public event Action<GameObject> ONEnemyClicked; //鼠标点击敌人的委托 
    public Texture2D point, doorway, attack, target, arrow;

    private RaycastHit _hitInfo;

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
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    private void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && _hitInfo.collider != null)
        {
            if (_hitInfo.collider.CompareTag("Ground"))
            {
                ONMouseClicked?.Invoke(_hitInfo.point); //如果ray碰到了地面则将点传到Unity的事件委托里
            }
            if (_hitInfo.collider.CompareTag("Enemy"))
            {
                ONEnemyClicked?.Invoke(_hitInfo.collider.gameObject); //将鼠标点击到的敌人对象传递到委托的方法里去
            }
            if (_hitInfo.collider.CompareTag("Attackable"))
            {
                ONEnemyClicked?.Invoke(_hitInfo.collider.gameObject);
            }

            if (_hitInfo.collider.CompareTag("Portal"))
            {
                ONMouseClicked?.Invoke(_hitInfo.point);
            }
        }
    }
}

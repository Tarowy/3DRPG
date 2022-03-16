using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> //单例模式的工具类，继承该类会自动生成T类型的单例
{
    private static T _instance;

    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;
    }

    public static bool IsInitialized => _instance == null;

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

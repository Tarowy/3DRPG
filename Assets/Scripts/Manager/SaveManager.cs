using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavaData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }

    public void SavaData()
    {
        SaveData(GameManager.Instance.playerStats.characterDataSo, GameManager.Instance.playerStats.name);
    }

    public void LoadData()
    {
        LoadData(GameManager.Instance.playerStats.characterDataSo, GameManager.Instance.playerStats.name);
    }

    /// <summary>
    /// 将数据转换为Json格式并使用PlayerPrefab保存
    /// JsonUtility.ToJson(data,true) 数据类型，是否需要美观点的格式
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name"></param>
    private void SaveData(Object data,string key)
    {
        var json = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key,json);
        //保存数据到磁盘中
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 将保存的Json数据写回到内存并覆盖当前的数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    private void LoadData(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            //按照data的数据类型将Json写回到data
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}

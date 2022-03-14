using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// 首个场景内不能有Manger或者必须未激活，否则传送门从另一个场景传送回来的时候，
/// 此场景已经激活的GameManger会覆盖掉DontDestroy的GameManger
/// 导致DontDestroy的GameManger被覆盖掉
/// 从而丢失DontDestroy本来的数据
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    private CinemachineFreeLook _cinemachineFreeLook;
    private List<IEndGameObserver> _endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // Debug.Log(playerStats + "--" + Time.time);
    }

    public void RegisterPlayer(CharacterStats player)
    {
        // Debug.Log("接收注册--" + Time.time);
        playerStats = player;
        // Debug.Log(playerStats+"--1 " + Time.time);

        var cineMachine = FindObjectOfType<CinemachineFreeLook>();

        if (cineMachine != null)
        {
            cineMachine.LookAt = playerStats.gameObject.transform.GetChild(2).transform;
            cineMachine.Follow = playerStats.gameObject.transform.GetChild(2).transform;
        }
        // Debug.Log(playerStats+"--2 " + Time.time);
    }   

    public void AddObserver(IEndGameObserver observer)
    {
        _endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        _endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var vObserver in _endGameObservers)
        {
            vObserver.EndNotify();
        }
    }
}

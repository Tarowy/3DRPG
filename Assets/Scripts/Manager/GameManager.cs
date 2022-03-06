using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    private List<IEndGameObserver> _endGameObservers = new List<IEndGameObserver>();

    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
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

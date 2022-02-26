using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates{ GUARD, PATROL, CHASE, DEAD}
[RequireComponent(typeof(NavMeshAgent))] //如果对象没有该组件会自动添加
public class EnemyController : MonoBehaviour
{
    public EnemyStates enemyStates;
    
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                break; 
            case EnemyStates.CHASE:
                break; 
            case EnemyStates.DEAD:
                break;
        }
    }
}

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
    [Header("Base Settings")] public float sightRadius;

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
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            Debug.Log("切换到追击模式");
        }
        
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

    private bool FoundPlayer()
    {
        //返回sightRadius范围内所有的物体，物体需要有collider才能检测到
        var overlapSphere = Physics.OverlapSphere(transform.position,sightRadius);
        foreach (var o in overlapSphere)
        {
            if (o.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}

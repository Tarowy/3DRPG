using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManger : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        MouseManager.Instance._onMouseClicked += MoveToTarget;
    }

    private void MoveToTarget(Vector3 target)
    {
        _navMeshAgent.destination = target;
    }
}

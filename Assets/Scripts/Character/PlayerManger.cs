using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManger : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        MouseManager.Instance._onMouseClicked += MoveToTarget;
    }

    private void Update()
    {
        SwitchAnimation();
    }

    private void SwitchAnimation()
    {
        _animator.SetFloat("speed",_navMeshAgent.velocity.sqrMagnitude); //将速度矢量转换为浮点数传入speed
        Debug.Log("speed:" + _navMeshAgent.velocity.sqrMagnitude);
    }

    private void MoveToTarget(Vector3 target)
    {
        _navMeshAgent.destination = target;
    }
}

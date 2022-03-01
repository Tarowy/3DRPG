using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManger : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private CharacterStats _characterStats;

    private GameObject _attackTarget;
    private float _lastAttackTime;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        MouseManager.Instance.ONMouseClicked += MoveToTarget;
        MouseManager.Instance.ONEnemyClicked += MoveToAttackTarget;
    }

    private void Update()
    {
        SwitchAnimation();
        _lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        _animator.SetFloat("speed",_navMeshAgent.velocity.sqrMagnitude); //将速度矢量转换为浮点数传入speed
        // Debug.Log("speed:" + _navMeshAgent.velocity.sqrMagnitude);
    }

    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines(); //为了可以在攻击的时候随时打断攻击动画
        _navMeshAgent.isStopped = false; //攻击敌人后agent会变为停止状态而无法行走,所以要置为false
        _navMeshAgent.destination = target;
    }

    private void MoveToAttackTarget(GameObject target)
    {
        if (target==null) { return; }
        _attackTarget = target;

        _navMeshAgent.isStopped = false;

        StartCoroutine(AttackTarget());
    }

    private IEnumerator AttackTarget()
    {
        transform.LookAt(_attackTarget.transform); //使角色的方向指向敌人

        //TODO:停止的距离会随着武器的长度而更改
        while (Vector3.Distance(_attackTarget.gameObject.transform.position, transform.position) > 1)
        {
            _navMeshAgent.destination = _attackTarget.transform.position;
            yield return null; //如果距离大于1就会一直靠近敌人
        }

        _navMeshAgent.isStopped = true; //靠近敌人后停止以攻击

        while (_lastAttackTime < 0)
        {
            _animator.SetTrigger("Attack");
            _lastAttackTime = 0.5f;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
        _characterStats.isCritical = Random.value < _characterStats.attackDataSo.criticalChance;

        StartCoroutine(AttackTarget());
    }

    private IEnumerator AttackTarget()
    {
        transform.LookAt(_attackTarget.transform); //使角色的方向指向敌人

        while (Vector3.Distance(_attackTarget.gameObject.transform.position, transform.position) >
               _characterStats.attackDataSo.attackRange) //停止的距离会随着武器的长短而改变
        {
            _navMeshAgent.destination = _attackTarget.transform.position;
            yield return null; //如果距离大于攻击距离就会一直靠近敌人
        }

        _navMeshAgent.isStopped = true; //靠近敌人后停止行动以攻击

        if (_lastAttackTime < 0)
        {
            _animator.SetBool("Critical", _characterStats.isCritical);
            _animator.SetTrigger("Attack");
            _lastAttackTime = _characterStats.attackDataSo.coolDown;
        }
    }
    
    //Animation Event
    public void Hit()
    {
        _characterStats.TakeDamage(this._characterStats, _attackTarget.GetComponent<CharacterStats>());
    }
    
}

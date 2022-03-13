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
    private float _stopDistance; //初始设定好的停止距离

    //动画变量
    private bool _isDeath;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _characterStats = GetComponent<CharacterStats>();

        _stopDistance = _navMeshAgent.stoppingDistance;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this._characterStats);
        
        MouseManager.Instance.ONMouseClicked += MoveToTarget;
        MouseManager.Instance.ONEnemyClicked += MoveToAttackTarget;
    }

    private void Update()
    {
        _isDeath = _characterStats.CurrentHealth == 0;
        if (_isDeath) //角色死亡之后就通知所有继承IEndGameNotify的对象
            GameManager.Instance.NotifyObservers();
        
        SwitchAnimation();
        _lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        _animator.SetFloat("speed",_navMeshAgent.velocity.sqrMagnitude); //将速度矢量转换为浮点数传入speed
        _animator.SetBool("Death", _isDeath);
        // Debug.Log("speed:" + _navMeshAgent.velocity.sqrMagnitude);
    }

    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines(); //为了可以在攻击的时候随时打断攻击动画
        if (_isDeath) return;

        _navMeshAgent.stoppingDistance = _stopDistance;
        _navMeshAgent.isStopped = false; //攻击敌人后agent会变为停止状态而无法行走,所以要置为false
        _navMeshAgent.destination = target;
    }

    private void MoveToAttackTarget(GameObject target)
    {
        if (target==null || _isDeath) return;
        _attackTarget = target;
        _navMeshAgent.stoppingDistance = _characterStats.attackDataSo.attackRange; //将停止距离替换为攻击距离，防止敌人的模型过大导致一直向前走

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
            Debug.Log("1："+"--"+Time.time);
            _navMeshAgent.destination = _attackTarget.transform.position;
            yield return null; //如果距离大于攻击距离就会一直靠近敌人
            Debug.Log("2："+"--"+Time.time);
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
        if (_attackTarget == null)
        {
            return;
        }
        if (_attackTarget.CompareTag("Attackable"))
        {
            if (_attackTarget.GetComponent<Rock>() &&
                _attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
            {
                _attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                _attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one; //刚开始的时候石头速度很小，这时会被其中的FixUPDATE重新置会HitNothing
                _attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
            }
        }
        else
        {
            if (_attackTarget != null)
            {
                _attackTarget.GetComponent<CharacterStats>().TakeDamage(this._characterStats, _attackTarget.GetComponent<CharacterStats>());
            }
        }
    }
    
}

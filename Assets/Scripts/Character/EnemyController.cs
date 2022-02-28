using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyStates{ GUARD, PATROL, CHASE, DEAD}
[RequireComponent(typeof(NavMeshAgent))] //如果对象没有该组件会自动添加
public class EnemyController : MonoBehaviour
{
    public EnemyStates enemyStates;
    
    private NavMeshAgent _navMeshAgent;
    private GameObject _attackTarget;
    private Animator _animator;

    [Header("Base Settings")] 
    public float sightRadius; //检测sightRadius半径范围内的物体
    private bool _isGuard; //此敌人是巡逻种类的还是站桩种类的
    private float _speed; //初始的速度
    private Vector3 _guardPos; //初始处于的位置
    public float lookAtTime; //到达目的地后的停留时间
    private float _remainLookAtTime;

    [Header("Patrol State")] 
    public float patrolRange;
    private Vector3 _wayPoint;
    
    //动画变量
    private bool _isChase;
    private bool _isWalk;
    private bool _isFollow;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _guardPos = transform.position;
        _speed = _navMeshAgent.speed;
        _remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        if (_isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
    }

    private void Update()
    {
        SwitchStates();
        SwitchAnimation();
    }

    private void SwitchAnimation()
    {
        _animator.SetBool("Walk", _isWalk);
        _animator.SetBool("Chase", _isChase);
        _animator.SetBool("Follow", _isFollow);
    }

    private void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                _isChase = false;
                _navMeshAgent.speed = _speed * 0.5f; //nav中使用乘法的开销比除法的开销要小

                if (Vector3.Distance(_wayPoint, transform.position) <= _navMeshAgent.stoppingDistance)
                { 
                    _isWalk = false;
                    if (_remainLookAtTime > 0) 
                        _remainLookAtTime -= Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    _isWalk = true;
                    _navMeshAgent.destination = _wayPoint;
                }

                break; 
            case EnemyStates.CHASE:
                //TODO:在攻击范围内则进行攻击
                
                _isWalk = false;
                _isChase = true;
                
                _navMeshAgent.speed = _speed; //追击的时候回到原来的速度
                if (FoundPlayer())
                {
                    _isFollow = true;
                    _navMeshAgent.destination = _attackTarget.transform.position;
                }
                else
                {
                    _isFollow = false;
                    _isChase = false;
                    if (_remainLookAtTime > 0)
                    {
                        _navMeshAgent.destination = transform.position; //拉脱之后仍会有一段余留速度，使其立马停止在原位
                        _remainLookAtTime -= Time.deltaTime;  
                    }
                    else if (_isGuard) 
                        enemyStates = EnemyStates.GUARD;
                    else 
                        enemyStates = EnemyStates.PATROL;
                }
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
                _attackTarget = o.gameObject;
                return true;
            }
        }

        _attackTarget = null;
        return false;
    }

    private void GetNewWayPoint()
    {
        _remainLookAtTime = lookAtTime; //复原停留时间
        
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomY = Random.Range(-patrolRange, patrolRange);

        //使用transform.position.y是为了防止y一直不变导致遇到坑洞的时候会在空中悬浮移动
        Vector3 random = new Vector3(_guardPos.x + randomX, transform.position.y, _guardPos.z + randomY);

        NavMeshHit hit; //navmeshhit中包含该点的信息
        //如果这个点是walkable的则返回该点，否则保持原地不动，areamask是可以碰撞到的agent中的areas层
        _wayPoint = NavMesh.SamplePosition(random, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void OnDrawGizmosSelected() //可以画出线条
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius); //有Wire则画的是线条，没有则是实心的
    }
}

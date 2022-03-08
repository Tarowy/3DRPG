using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyStates{ GUARD, PATROL, CHASE, DEAD}
[RequireComponent(typeof(NavMeshAgent))] //如果对象没有该组件会自动添加
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    public EnemyStates enemyStates;
    
    private NavMeshAgent _navMeshAgent;
    protected GameObject attackTarget;
    private Animator _animator;
    private CharacterStats _characterStats;
    private Collider _collider;

    [Header("Base Settings")] 
    public float sightRadius; //检测sightRadius半径范围内的物体
    public bool isGuard; //此敌人是巡逻种类的还是站桩种类的
    public float lookAtTime; //到达目的地后的停留时间
    
    private float _speed; //初始的速度
    private Vector3 _guardPos; //初始处于的位置
    private Quaternion _guardRotation; //初始的旋转角度
    private float _remainLookAtTime;
    private float _lastAttackTime;

    [Header("Patrol State")] 
    public float patrolRange;
    private Vector3 _wayPoint;
    
    //动画变量
    private bool _isChase;
    private bool _isWalk;
    private bool _isFollow;
    private bool _isDeath;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _characterStats = GetComponent<CharacterStats>();
        _collider = GetComponent<Collider>();
        
        _guardPos = transform.position;
        _speed = _navMeshAgent.speed;
        _remainLookAtTime = lookAtTime;
        _guardRotation = transform.rotation;
    }
    
    private void OnDisable() //OnDisable是在销毁之后才执行的，OnDestroy是在销毁时执行的
    {
        if (!GameManager.IsInitialized) return; //如果GameManager还未初始化，执行下面的会导致报错
        GameManager.Instance.RemoveObserver(this);
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        GameManager.Instance.AddObserver(this);
    }

    private void Update()
    {
        _isDeath = _characterStats.CurrentHealth == 0;
        SwitchStates();
        SwitchAnimation(); 
        _lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        _animator.SetBool("Walk", _isWalk);
        _animator.SetBool("Chase", _isChase);
        _animator.SetBool("Follow", _isFollow);
        _animator.SetBool("Critical", _characterStats.isCritical);
        _animator.SetBool("Death", _isDeath);
    }

    private void SwitchStates()
    {
        if (_isDeath)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                if (transform.position != _guardPos)
                {
                    _isWalk = true;
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.destination = _guardPos;
                    
                    //SqrMagnitude的开销比distance小
                    Debug.Log("SqrMagnitude:"+Vector3.SqrMagnitude(_guardPos - transform.position));
                    if (Vector3.SqrMagnitude(_guardPos - transform.position) <= _navMeshAgent.stoppingDistance)
                    {
                        _isWalk = false;
                        //利用线性插值缓慢转回到初始角度
                        transform.rotation = Quaternion.Lerp(transform.rotation, _guardRotation, 0.01f);
                    }
                }
                break;
            case EnemyStates.PATROL:
                _isChase = false;
                _navMeshAgent.speed = _speed * 0.5f; //不追击的时候速度比较慢（nav中使用乘法的开销比除法的开销要小）
                // _navMeshAgent.isStopped = false;

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
                _isWalk = false;
                _isChase = true;
                
                _navMeshAgent.speed = _speed; //追击的时候回到原来的速度
                if (FoundPlayer())
                {
                    _isFollow = true;
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.destination = attackTarget.transform.position;
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
                    else if (isGuard) 
                        enemyStates = EnemyStates.GUARD;
                    else 
                        enemyStates = EnemyStates.PATROL;
                }
                
                //玩家在攻击范围内则进行攻击
                if (TargetInSkillRange() || TargetInAttackRange()) //如果在技能范围内则不攻击，一直释放技能
                {
                    _isFollow = false;
                    _navMeshAgent.isStopped = true;
                    if (_lastAttackTime<=0)
                    {
                        _lastAttackTime = _characterStats.attackDataSo.coolDown;
                        _characterStats.isCritical = Random.value <= _characterStats.attackDataSo.criticalChance;
                        Attack();
                    }
                }
                
                break; 
            case EnemyStates.DEAD:
                _collider.enabled = false;
                // _navMeshAgent.enabled = false;
                //在挂载了StopAgent的动画执行StopAgent中的update的时候，该对象突然死亡会导致StopAgent丢失Agent的对象
                _navMeshAgent.radius = 0;
                Destroy(gameObject, 2f);
                break;
        }
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);

        if (TargetInSkillRange())
        {
            _animator.SetTrigger("Skill");
        }else if (TargetInAttackRange())
        {
            _animator.SetTrigger("Attack");
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
                attackTarget = o.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    private bool TargetInAttackRange()
    {
        if (attackTarget is null) return false;
        return Vector3.Distance(attackTarget.transform.position, transform.position) <=
               _characterStats.attackDataSo.attackRange; //如果agent的stopDistance大于attackRange就会导致无法攻击
    }

    private bool TargetInSkillRange()
    {
        if (attackTarget is null) return false;
        return Vector3.Distance(attackTarget.transform.position, transform.position) <=
               _characterStats.attackDataSo.skillRange;
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
    
    //Animation Event
    public void Hit()
    {
        Debug.Log(gameObject.name+"攻击");
        if (attackTarget is null) return; //由于敌人获取玩家的对象是自动的，所以相比于玩家的手动控制可能会丢失对象
        
        _characterStats.TakeDamage(_characterStats, attackTarget.GetComponent<CharacterStats>());
    }

    public void EndNotify()
    {
        //胜利动画
        _animator.SetBool("Win", true);
        //停止所有移动
        //停止Agent
        _isFollow = false;
        _isChase = false;
        attackTarget = null;

        _navMeshAgent.isStopped = true;
    }
}

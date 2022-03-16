using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer,HitEnemy,HitNothing }
    private Rigidbody _rigidbody;
    public RockStates rockStates;

    [Header("Basic Settings")] 
    public float force;
    public GameObject target;
    private Vector3 _direction;
    public int damage;
    public GameObject rockBreak;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.velocity=Vector3.one; //刚产生石头的时候，其速度也很小，会被提前转变为HitNothing状态
        
        FlyToTarget();
        rockStates = RockStates.HitPlayer;
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.sqrMagnitude < 1f)
        {
            rockStates = RockStates.HitNothing; //如果石头没砸上玩家，那么在速度很小的时候也会改变状态
        }
    }

    public void FlyToTarget()
    {
        if (target == null) //如果再投石动画发生的过程中玩家摆脱了敌人，会导致target为空值从而石头会直接落下
            target = FindObjectOfType<PlayerManger>().gameObject;
        
        _direction = (target.transform.position - transform.position + Vector3.up).normalized;
        _rigidbody.AddForce(_direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = _direction * force;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>()
                        .TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());

                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem>()) //GetComponent如果获取不到指定的组件就会返回false
                {
                    var characterStats = other.gameObject.GetComponent<CharacterStats>();
                    characterStats.TakeDamage(damage, characterStats);

                    Instantiate(rockBreak,transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}

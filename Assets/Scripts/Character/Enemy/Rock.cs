using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Basic Settings")] 
    public float force;
    public GameObject target;
    private Vector3 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        FlyToTarget();
    }

    public void FlyToTarget()
    {
        if (target == null) //如果再投石动画发生的过程中玩家摆脱了敌人，会导致target为空值从而石头会直接落下
            target = GameObject.Find("Player");
        
        _direction = (target.transform.position - transform.position + Vector3.up).normalized;
        _rigidbody.AddForce(_direction * force, ForceMode.Impulse);
    }
}

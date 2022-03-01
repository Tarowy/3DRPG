using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName = "Character Stats/Attack")]
public class AttackData_SO : ScriptableObject
{
    public float attackDamage;
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;
    public float criticalMultiply; //暴击倍率
    public float criticalChance;
}

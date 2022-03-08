using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")] 
    public float kickForce;

    public void KickOff() //动画事件，将玩家推开
    {
        if (attackTarget != null)
        {
            Debug.Log("推");
            transform.LookAt(attackTarget.transform);
            
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}

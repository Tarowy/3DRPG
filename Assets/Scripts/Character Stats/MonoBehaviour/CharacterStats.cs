using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack; 
    public CharacterData_SO tempCharacterDataSo; //当创建敌人的时候就会从中复制一份数据，防止敌人间的数据共用
    
    public CharacterData_SO characterDataSo; //从指定的脚本中读取数据
    public AttackData_SO attackDataSo;
    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        characterDataSo = (CharacterData_SO) Instantiate(tempCharacterDataSo);
    }

    #region Read From Data_SO
    public int MaxHealth
    {
        get => characterDataSo != null ? characterDataSo.maxHealth : 0;
        set => characterDataSo.maxHealth = value;
    }

    public int CurrentHealth
    {
        get => characterDataSo != null ? characterDataSo.currentHealth : 0;
        set => characterDataSo.currentHealth = value;
    }
    
    public int BaseDefence
    {
        get => characterDataSo != null ? characterDataSo.baseDefence : 0;
        set => characterDataSo.baseDefence = value;
    }
    
    public int CurrentDefence
    {
        get => characterDataSo != null ? characterDataSo.currentDefence : 0;
        set => characterDataSo.currentDefence = value;
    }
    #endregion //可以折叠区域代码，指明里面的是什么内容

    #region Character Combat

    public void TakeDamage(CharacterStats attacker, CharacterStats defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage()-defender.CurrentDefence, 0); //防止对手的防御力大于攻击者的攻击力出现负值导致对手反而加血
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - damage, 0); //防止血量变为负数

        if (attacker.isCritical)
        {
            Debug.Log("暴击");
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }

        defender.UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            attacker.characterDataSo.UpdateExp(characterDataSo.killPoint);
        }
    }

    public void TakeDamage(int damage,CharacterStats defender)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - currentDamage, 0);
        
        defender.UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
    }

    private int CurrentDamage()
    {
        int coreDamage = Random.Range(attackDataSo.minDamage, attackDataSo.maxDamage);
        return isCritical ? (int)(coreDamage * attackDataSo.criticalMultiply) : coreDamage;
    }

    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData_SO tempCharacterDataSo; //当创建敌人的时候就会从中复制一份数据，防止敌人间的数据共用
    public AttackData_SO tempAttackDataSo;

    public CharacterData_SO characterDataSo; //从指定的脚本中读取数据
    public AttackData_SO attackDataSo;

    public Transform weaponSlot;
    public RuntimeAnimatorController tempAnimator;

    [HideInInspector] public bool isCritical;

    private void Awake()
    {
        characterDataSo = (CharacterData_SO) Instantiate(tempCharacterDataSo);
        attackDataSo = Instantiate(tempAttackDataSo);
        tempAnimator = GetComponent<Animator>().runtimeAnimatorController;
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
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0); //防止对手的防御力大于攻击者的攻击力出现负值导致对手反而加血
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - damage, 0); //防止血量变为负数

        if (attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }

        defender.UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            StartCoroutine(attacker.characterDataSo.UpdateExp(characterDataSo.killPoint));
        }
    }

    public void TakeDamage(int damage, CharacterStats defender)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - currentDamage, 0);

        defender.UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        //直接击杀敌人也可以
        if (CurrentHealth <= 0)
        {
            GameManager.Instance.playerStats.characterDataSo.UpdateExp(characterDataSo.killPoint);
        }
    }

    private int CurrentDamage()
    {
        int coreDamage = Random.Range(attackDataSo.minDamage, attackDataSo.maxDamage);
        return isCritical ? (int) (coreDamage * attackDataSo.criticalMultiply) : coreDamage;
    }

    #endregion

    #region Weapon

    public void ChangeWeapon(Item_SO weapon)
    {
        UnEquipmentWeapon();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(Item_SO itemSo)
    {
        if (itemSo.weaponPrefab != null)
        {
            var weapon = Instantiate(itemSo.weaponPrefab, weaponSlot);
            weapon.transform.localPosition = new Vector3(0, 0, 0);
            weapon.transform.localEulerAngles = new Vector3(-180, 0, 0);
        }

        //应用武器数据
        attackDataSo.ApplyWeaponData(itemSo.attackDataSo);
        //切换动画模组
        GetComponent<Animator>().runtimeAnimatorController = itemSo.weaponAnimator;
    }

    /// <summary>
    /// 卸下武器
    /// </summary>
    public void UnEquipmentWeapon()
    {
        //销毁武器槽位的所有子物体
        if (weaponSlot.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.childCount; i++)
            {
                Destroy(weaponSlot.GetChild(i).gameObject);
            }
        }

        //还原原始攻击数据
        attackDataSo.ApplyWeaponData(tempAttackDataSo);
        //还原动画模组
        GetComponent<Animator>().runtimeAnimatorController = tempAnimator;
    }

    #endregion

    #region 回血相关

    public bool ApplyHealth(int amount)
    {
        //满血就不回复
        if (CurrentHealth==MaxHealth)
        {
            return false;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        return true;
    }

    #endregion
}
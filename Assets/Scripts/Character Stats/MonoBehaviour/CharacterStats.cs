using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterDataSo; //从指定的脚本中读取数据

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
}

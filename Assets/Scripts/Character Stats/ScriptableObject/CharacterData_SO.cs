using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject //ScriptableObject可以在文件夹创建特性中指明的资源文件方便直接管理本类中的字段
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")] 
    public int killPoint;
    
    [Header("Level")] 
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff; //每升一级增加所需要的经验值

    public float LevelMultiply => 1 + (currentLevel - 1) * levelBuff;

    public IEnumerator UpdateExp(int point)
    {
        currentExp += point;

        while (currentExp > baseExp)
        {
            currentExp -= baseExp;
            LevelUp();
            yield return null;
        }
    }

    private void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp = (int) (baseExp * LevelMultiply);

        maxHealth += 10;
        currentHealth = maxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject item;
        [Range(0, 1)] 
        public float weight; //物品的掉落概率
    }

    public LootItem[] lootItems;

    
    /// <summary>
    /// 根据概率掉落物品
    /// </summary>
    public void SpawnLoot()
    {
        float currentValue = Random.value;
        Debug.Log(currentValue);

        foreach (var lootItem in lootItems)
        {
            //当生成的随机数字小于概率的时候就掉落
            if (currentValue <= lootItem.weight)
            {
                var instantiate = Instantiate(lootItem.item);
                instantiate.transform.position = transform.position + Vector3.up * 2;
            }
        }
    }
}

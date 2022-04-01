using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest",menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string targetName; //任务需要的物品名称
        public int requireAmount;
        public int currentAmount;
    }

    public string questName;
    [TextArea]
    public string description;

    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;

    //同一个任务可以要求多个不同的物品
    public List<QuestRequire> questRequires = new List<QuestRequire>();
    //任务奖励
    public List<InventoryItem> rewardItems = new List<InventoryItem>();
}

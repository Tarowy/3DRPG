using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
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
    [TextArea] public string description;

    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;

    //同一个任务可以要求多个不同的物品
    public List<QuestRequire> questRequires = new List<QuestRequire>();

    //任务奖励
    public List<InventoryItem> rewardItems = new List<InventoryItem>();

    /// <summary>
    /// 使用Linq查询该questRequires中QuestRequire的当前数量满足需求数量的结果
    /// </summary>
    public void CheckQuestProgress()
    {
        var finishRequire = questRequires.Where(q => q.requireAmount <= q.currentAmount);
        //当前数量满足需求数量的结果集总数与任务集合总数相等则说明该任务已完成
        isCompleted = finishRequire.Count() == questRequires.Count;
    }


    /// <summary>
    /// 奖励数量是正数表示是要给予的奖励，直接添加到背包即可
    /// 奖励数量是负数表示是任务所需的物品：
    ///     1.背包或快捷栏里的数量大于等于需求数量则直接从背包里减去需求数量
    ///     2.背包或快捷栏有该物品但数量不足，那么需求数量就减去所拥有的数量，再将背包里的物品数量清零
    ///     3.背包里有，快捷栏里也有那么需要依次减去背包和快捷栏的数量知道符合要求
    /// </summary>
    public void GiveRewards()
    {
        foreach (var rewardItem in rewardItems)
        {
            //如果该奖励物品的数量小于0说明是要从背包里减去
            if (rewardItem.amount < 0)
            {
                //获取绝对值方便计算
                var requireAmount = Mathf.Abs(rewardItem.amount);

                //如果背包里有该物品
                if (InventoryManager.Instance.QuestItemInBag(rewardItem.itemSo) != null)
                {
                    //背包数量少于或等于需求数量
                    if (InventoryManager.Instance.QuestItemInBag(rewardItem.itemSo).amount <= requireAmount)
                    {
                        requireAmount -= InventoryManager.Instance.QuestItemInBag(rewardItem.itemSo).amount;
                        //无论是少于还是等于需求数量都会拿掉所有已经拥有的该物品数量
                        InventoryManager.Instance.QuestItemInBag(rewardItem.itemSo).amount = 0;

                        //背包的数量不够则还要再从快捷栏里拿取该物品，如果快捷栏有则减去所有需要的剩余数量
                        if (InventoryManager.Instance.QuestItemInAction(rewardItem.itemSo) != null)
                        {
                            InventoryManager.Instance.QuestItemInAction(rewardItem.itemSo).amount -= requireAmount;
                        }
                    }
                    //背包数量大于需求数量则直接减去需求数量
                    else
                    {
                        InventoryManager.Instance.QuestItemInBag(rewardItem.itemSo).amount -= requireAmount;
                    }
                }
                //没有物品直接从快捷栏里减去
                else
                {
                    InventoryManager.Instance.QuestItemInAction(rewardItem.itemSo).amount -= requireAmount;
                }
            }
            //如果大于0就是要给予奖励
            else if (rewardItem.amount > 0)
            {
                InventoryManager.Instance.inventoryData.AddItem(rewardItem.itemSo, rewardItem.amount);
            }
        }

        //给予奖励之后跟新物品栏UI
        InventoryManager.Instance.inventoryUI.RefreshUI();
        InventoryManager.Instance.actionUI.RefreshUI();
    }

    //获取该任务所需要的所有物品的名字的列表
    public List<string> GetQuestRequireName()
    {
        return questRequires.Select(require => require.targetName).ToList();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    //一个QuestTask包含了一个NPC的所有任务信息
    public class QuestTask
    {
        public QuestData_SO questDataSo; //该QuestTask的任务信息

        public bool IsStarted
        {
            get => questDataSo.isStarted;
            set => questDataSo.isStarted = value;
        }

        public bool IsCompleted
        {
            get => questDataSo.isCompleted;
            set => questDataSo.isCompleted = value;
        }

        public bool IsFinished
        {
            get => questDataSo.isFinished;
            set => questDataSo.isFinished = true;
        }
    }

    public List<QuestTask> questTaskList = new List<QuestTask>();

    /// <summary>
    /// 更新任务进度：
    ///     1.满足需求的敌人死亡后 数量+
    ///     2.拾取到满足需求的物品 数量+
    ///     3.使用掉了满足需求的物品 数量-
    /// 寻找questTaskList里有没有QuestTask的questDataSo的名字是targetName的，如果是就改变数量
    /// </summary>
    /// <param name="targetName">任务需求的物品的名字</param>
    /// <param name="amount">任务需求的物品需要增加数量</param>
    public void UpdateQuestProgress(string targetName, int amount)
    {
        //由于可能会承接多个任务，且多个任务需要的是同一个东西，那么就需要改变每个任务的该物品的数量
        foreach (var task in questTaskList)
        {
            var matchTask = task.questDataSo.questRequires.Find(q => q.targetName.Equals(targetName));
            if (matchTask != null)
            {
                matchTask.currentAmount += amount;
            }

            //每次更新任务进度都要检查一下该任务是否已经完成
            task.questDataSo.CheckQuestProgress();
        }
    }

    /// <summary>
    /// 使用Linq寻找QuestTask中有没有questDataSo这个任务
    /// </summary>
    /// <param name="questDataSo">任务包含的信息</param>
    /// <returns></returns>
    public bool HaveQuest(QuestData_SO questDataSo)
    {
        if (questDataSo != null)
        {
            return questTaskList.Any(q => questDataSo.questName.Equals(q.questDataSo.questName));
        }

        return false;
    }

    /// <summary>
    /// 使用Linq从QuestTask中获取与questDataSo一样的questDataSo
    /// </summary>
    /// <param name="questDataSo">需要获取的任务信息</param>
    /// <returns></returns>
    public QuestTask GetTask(QuestData_SO questDataSo)
    {
        return questTaskList.Find(q => q.questDataSo.questName.Equals(questDataSo.questName));
    }
}
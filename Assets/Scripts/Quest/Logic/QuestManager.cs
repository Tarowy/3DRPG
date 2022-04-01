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

    public bool HaveQuest(QuestData_SO questDataSo)
    {
        if (questDataSo != null)
        {
            return questTaskList.Any(q => questDataSo.questName.Equals(q.questDataSo.questName));
        }

        return false;
    }

    public QuestTask GetTask(QuestData_SO questDataSo)
    {
        return questTaskList.Find(q => q.questDataSo.questName.Equals(questDataSo.questName));
    }
}
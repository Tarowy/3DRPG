using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private DialogueController _controller;
    private QuestData_SO _currentQuestDataSo;

    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishDialogue;

    #region 获取任务的状态

    //任务开始时
    public bool IsStarted => QuestManager.Instance.HaveQuest(_currentQuestDataSo) &&
                             QuestManager.Instance.GetTask(_currentQuestDataSo).IsStarted;
    
    //任务刚结束时
    public bool IsComplete => QuestManager.Instance.HaveQuest(_currentQuestDataSo) &&
                              QuestManager.Instance.GetTask(_currentQuestDataSo).IsCompleted;

    //任务已经完全结束时
    public bool IsFinished => QuestManager.Instance.HaveQuest(_currentQuestDataSo) &&
                              QuestManager.Instance.GetTask(_currentQuestDataSo).IsFinished;

    #endregion


    private void Awake()
    {
        _controller = GetComponent<DialogueController>();
    }

    private void Start()
    {
        _controller.currentDialogueDataSo = startDialogue;
        _currentQuestDataSo = _controller.currentDialogueDataSo.questInThisDialogueDataSo;
    }

    private void Update()
    {
        //因为NPC更换对话数据是Update执行的所以会即时根据任务数据来更换对话，所以无需储存NPC当前的对话数据
        if (IsStarted)
        {
            //已经承接了任务且完成了，就切换到做完了任务的对话
            if (IsComplete)
            {
                _controller.currentDialogueDataSo = completeDialogue;
            }
            //已经承接了任务但未完成，就切换到承接了任务的对话
            else
            {
                _controller.currentDialogueDataSo = progressDialogue;
            }
        }

        //完成了任务之后的对话
        if (IsFinished)
        {
            _controller.currentDialogueDataSo = finishDialogue;
        }
    }
}
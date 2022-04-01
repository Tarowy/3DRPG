using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    private Button _thisButton;
    private DialoguePiece currentDialoguePiece;
    public string nextPieceID;
    private bool _takeQuest;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(ClickOption);
    }

    /// <summary>
    /// 更新按钮的信息
    /// </summary>
    /// <param name="piece">当前的对话信息</param>
    /// <param name="option">该选项所包含的信息</param>
    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        currentDialoguePiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        _takeQuest = option.taskQuest; //判断当前选项是不是可以承接任务
    }

    public void ClickOption()
    {
        if (currentDialoguePiece.questDataSo != null)
        {
            //直接生成一个任务的复制品DataSo防止覆盖原来的数据
            var newTask = new QuestManager.QuestTask()
                {questDataSo = Instantiate(currentDialoguePiece.questDataSo)};
            
            if (_takeQuest)
            {
                //判断任务列表是否有这个任务
                if (QuestManager.Instance.HaveQuest(newTask.questDataSo))
                {
                    //有这个任务且完成就给予奖励
                    
                }
                else
                {
                    //没有这个任务就加入此任务加入
                    QuestManager.Instance.questTaskList.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questDataSo).IsStarted = true;
                }
            }
        }

        if (nextPieceID.Equals(""))
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
        else
        {
            //让对话框的内容变为要跳转到的下一条的对话的内容
            DialogueUI.Instance.UpdateMainText(
                DialogueUI.Instance.currentDialogueDataSo.dialogueDictionary[nextPieceID]);
        }
    }
}
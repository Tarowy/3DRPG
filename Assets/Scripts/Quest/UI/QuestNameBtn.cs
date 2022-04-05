using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameBtn : MonoBehaviour
{
    public Text questNameText;
    public QuestData_SO currentQuestDataSo;
    public Text questContentText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    /// <summary>
    /// 将任务列表的数据一一赋值到按钮上
    /// </summary>
    /// <param name="questDataSo">任务列表的数据</param>
    public void InitQuestNameBtn(QuestData_SO questDataSo)
    {
        currentQuestDataSo = questDataSo;

        questNameText.text = currentQuestDataSo.isCompleted
            ? currentQuestDataSo.questName + "(已完成)"
            : currentQuestDataSo.questName;
    }

    private void UpdateQuestContent()
    {
        questContentText.text = currentQuestDataSo.description;
        QuestUI.Instance.SetupRequireList(currentQuestDataSo);

        QuestUI.Instance.DestroyRewardList();
        foreach (var inventoryItem in currentQuestDataSo.rewardItems)
        {
            QuestUI.Instance.SetupRewardList(inventoryItem.itemSo, inventoryItem.amount);
        }
    }
}

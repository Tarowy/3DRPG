using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")] public GameObject questPanel;
    public ItemToolTip itemToolTip;
    private bool _isOpened;

    [Header("Quest Name")] public RectTransform questListTransform;
    public QuestNameBtn questNameBtn;

    [Header("Text Content")] public Text questContentText;

    [Header("Requirement")] public RectTransform requireTransform;
    public QuestRequirement questRequirement;

    [Header("RewardPanel")] public RectTransform rewardTransform;
    public ItemUI itemUI;

    private void Start()
    {
        InitQuestPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _isOpened = !_isOpened;
            questPanel.SetActive(_isOpened);
            questContentText.text = String.Empty;
        }
    }

    private void InitQuestPanel()
    {
        //销毁所有子元素
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
    }

    public IEnumerator CreateQuestNameBtn()
    {
        //重新创建任务列表
        foreach (var questTask in QuestManager.Instance.questTaskList)
        {
            QuestNameBtn newTask;
            yield return newTask = Instantiate(questNameBtn, questListTransform);
            newTask.InitQuestNameBtn(questTask.questDataSo);
            newTask.questContentText = this.questContentText;
        }
    }

    public void SetupRequireList(QuestData_SO questDataSo)
    {
        //防止出现上一次的任务需求
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var require in questDataSo.questRequires)
        {
            var requireList = Instantiate(questRequirement,requireTransform);
            requireList.UpdateRequirement(require.targetName, require.currentAmount, require.requireAmount);
        }
    }
}
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
    public VerticalLayoutGroup verticalLayoutGroup;
    public CanvasGroup canvasGroup;

    [Header("Quest Name")] public RectTransform questListTransform;
    public QuestNameBtn questNameBtn;

    [Header("Text Content")] public Text questContentText;

    [Header("Requirement")] public RectTransform requireTransform;
    public QuestRequirement questRequirement;

    [Header("RewardPanel")] public RectTransform rewardTransform;
    public ItemUI itemUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ShowQuestList());
        }
    }

    /// <summary>
    /// 显示任务的UI，由于verticalLayoutGroup加载不及时会导致布局紊乱，需要做些处理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowQuestList()
    {
        _isOpened = !_isOpened;
        questPanel.SetActive(_isOpened);

        if (!_isOpened)
        {
            itemToolTip.gameObject.SetActive(false);
            verticalLayoutGroup.enabled = false;
            canvasGroup.alpha = 0;
            yield break;
        }

        questContentText.text = String.Empty;
        InitQuestPanel();
        //等所有任务名称按钮创建完之后等这一帧结束再打开verticalLayoutGroup
        yield return new WaitForEndOfFrame();

        /*
         * 需要等所有任务列表加载完之后才能启用verticalLayoutGroup，否则布局会乱
         */
        verticalLayoutGroup.enabled = true;

        /*
         * 由于上一步需要等所有任务列表加载完之后才能启用verticalLayoutGroup，有一帧的时间使混乱布局
         * 虽然时间很短，但还是容易发现，所以可以先将透明度设置为0，启用verticalLayoutGroup后再设置为不透明
         */
        canvasGroup.alpha = 1;
    }

    private void InitQuestPanel()
    {
        DestroyQuestList();
        DestroyRequireList();
        DestroyRewardList();

        //重新创建任务列表
        foreach (var questTask in QuestManager.Instance.questTaskList)
        {
            QuestNameBtn newTask = Instantiate(questNameBtn, questListTransform);
            newTask.InitQuestNameBtn(questTask.questDataSo);
            newTask.questContentText = this.questContentText;
        }
    }

    public void SetupRequireList(QuestData_SO questDataSo)
    {
        //防止出现上一次的任务需求
        DestroyRequireList();

        foreach (var require in questDataSo.questRequires)
        {
            var requireList = Instantiate(questRequirement, requireTransform);
            //如果该任务已经完成则将任务名称变为灰色
            if (questDataSo.isFinished)
            {
                requireList.UpdateRequirement(require.targetName, true);
                continue;
            }

            requireList.UpdateRequirement(require.targetName, require.currentAmount, require.requireAmount);
        }
    }

    /// <summary>
    /// 创建奖励栏UI
    /// </summary>
    /// <param name="itemSo">奖励栏的物品</param>
    /// <param name="amount">奖励的数量</param>
    public void SetupRewardList(Item_SO itemSo, int amount)
    {
        var rewardItem = Instantiate(itemUI, rewardTransform);
        rewardItem.SetUpItemUI(itemSo, amount);
    }

    #region 销毁原本存在的UI，防止布局出错

    //销毁任务栏的按钮
    public void DestroyQuestList()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
    }

    //销毁需求列表
    public void DestroyRequireList()
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
    }

    //销毁奖励栏位
    public void DestroyRewardList()
    {
        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
    }

    #endregion
}
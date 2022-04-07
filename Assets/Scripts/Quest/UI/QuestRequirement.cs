using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text _requireName;
    private Text _progressNumber;

    private void Awake()
    {
        _requireName = GetComponent<Text>();
        _progressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void UpdateRequirement(string requireName, int currentAmount, int totalAmount)
    {
        _requireName.text = requireName;
        _progressNumber.text = currentAmount + " / " + totalAmount;
    }

    public void UpdateRequirement(string requireName, bool isFinished)
    {
        if (isFinished)
        {
            _requireName.text = requireName;
            _progressNumber.text = "完成";
            _requireName.color = Color.grey;
            _progressNumber.color = Color.grey;
        }
    }
}
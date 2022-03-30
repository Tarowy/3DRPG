using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")] public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;

    [Header("Options")] public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")] public DialogueData_SO currentDialogueDataSo;
    public int currentIndex;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    private void ContinueDialogue()
    {
        if (currentIndex < currentDialogueDataSo.dialoguePieces.Count)
        {
            UpdateMainText(currentDialogueDataSo.dialoguePieces[currentIndex]);
        }
        else
        {
            //如果没有多余的对话选项就关闭对话框
            dialoguePanel.SetActive(false);
        }
    }

    public void UpdateDialogueData(DialogueData_SO dialogueDataSo)
    {
        currentDialogueDataSo = dialogueDataSo;
        currentIndex = 0;
    }

    public void UpdateMainText(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;

        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        //文本需要先置空，否则DoTween的显示方式会残留上一次的文字
        mainText.text = "";
        //DoTween插件，控制显示一段文字需要多长时间
        mainText.DOText(piece.text, 1f);

        //如果该条对话有选项就得禁用nextButton
        if (piece.options.Count == 0 && currentDialogueDataSo.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //如果直接禁用Button会打乱布局
            // nextButton.gameObject.SetActive(false);
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        //创建Option
        CreateOption(piece);
    }

    public void CreateOption(DialoguePiece piece)
    {
        //销毁当前Piece下的所有子物体
        if (optionPanel.childCount > 0)
        {
            Debug.Log("销毁");
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }

        //生成对应数目的Option
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece, piece.options[i]);
        }
    }
}
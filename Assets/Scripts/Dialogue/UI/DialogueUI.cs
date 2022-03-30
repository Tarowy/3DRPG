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
            nextButton.gameObject.SetActive(true);
            currentIndex++;
        }
        else
        {
            nextButton.gameObject.SetActive(false);
        }
    }
}
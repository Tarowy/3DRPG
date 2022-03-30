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
    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentDialoguePiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
    }

    public void ClickOption()
    {
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

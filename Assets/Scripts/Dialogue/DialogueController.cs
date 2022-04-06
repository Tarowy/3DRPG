using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentDialogueDataSo;
    public bool canTalk;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentDialogueDataSo != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetMouseButtonDown(1))
        {
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        //将自己的对话数据传到对话面板上去
        DialogueUI.Instance.UpdateDialogueData(currentDialogueDataSo);
        DialogueUI.Instance.UpdateMainText(currentDialogueDataSo.dialoguePieces[0]);
    }
}
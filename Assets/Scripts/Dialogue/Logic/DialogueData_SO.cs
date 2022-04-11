using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueDictionary = new Dictionary<string, DialoguePiece>();

    public QuestData_SO questInThisDialogueDataSo;

    //只有在Unity编译器中改变该SO的内容才会执行
#if UNITY_EDITOR
    //只要该SO的内容被改变就执行该方法
    private void OnValidate()
    {
        dialogueDictionary.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueDictionary.ContainsKey(piece.id))
            {
                dialogueDictionary.Add(piece.id, piece);
            }
        }
    }
#else
    void Awake() //保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    {
        dialogueDictionary.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueDictionary.ContainsKey(piece.id))
            {
                dialogueDictionary.Add(piece.id, piece);
            }
        }
    }
#endif
}
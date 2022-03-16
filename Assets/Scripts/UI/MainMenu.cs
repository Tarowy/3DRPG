using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button _startBtn;
    private Button _continueBtn;
    private Button _quitBtn;

    private void Awake()
    {
        _startBtn = transform.GetChild(1).GetComponent<Button>();
        _continueBtn = transform.GetChild(2).GetComponent<Button>();
        _quitBtn = transform.GetChild(3).GetComponent<Button>();

        _startBtn.onClick.AddListener(NewGame);
        _continueBtn.onClick.AddListener(Continue);
        _quitBtn.onClick.AddListener(Quit);
    }

    private void NewGame()
    {
        PlayerPrefs.DeleteAll();
        //转换场景
        SceneController.Instance.TransitionToFirstLevel();
    }
    
    private void Continue()
    {
        SceneController.Instance.TransitionToLoadLevel();
    }
    
    private void Quit()
    {
        Application.Quit();
    }
}

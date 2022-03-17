using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private PlayableDirector _director;
    
    private Button _startBtn;
    private Button _continueBtn;
    private Button _quitBtn;

    private void Awake()
    {
        _startBtn = transform.GetChild(1).GetComponent<Button>();
        _continueBtn = transform.GetChild(2).GetComponent<Button>();
        _quitBtn = transform.GetChild(3).GetComponent<Button>();

        _startBtn.onClick.AddListener(PlayTimeLine);
        _continueBtn.onClick.AddListener(Continue);
        _quitBtn.onClick.AddListener(Quit);

        _director = FindObjectOfType<PlayableDirector>();
        //播放完动画执行事件
        _director.stopped += NewGame;
    }

    private void PlayTimeLine()
    {
        _director.Play();
    }

    /// <summary>
    /// 函数参数必须要有PlayableDirector类型才能被director挂载
    /// </summary>
    /// <param name="director"></param>
    private void NewGame(PlayableDirector director)
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

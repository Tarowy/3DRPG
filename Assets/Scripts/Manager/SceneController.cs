using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;
    public FadeCanvas fadeCanvasPrefab;
    private bool _fadeFinished;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        _fadeFinished = true;
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
    }

    /// <summary>
    /// 别的传送门调用此方法，将自己的目的传送门的信息传进来
    /// portal中的destinationTag是目的传送门的标签
    /// DestinationPosition中的destinationTag是该传送门本身的标签
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                //SceneManager.GetActiveScene()可以获取当前激活场景的信息
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                playerPrefab.GetComponent<NavMeshAgent>().isStopped = true;
                break;
            
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    /// <summary>
    /// 使用协程异步加载另一个场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="destinationTag"></param>
    /// <returns></returns>
    private IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        if (!sceneName.Equals(SceneManager.GetActiveScene().name))
        {
            SaveManager.Instance.SavaData();
            //保存背包数据
            InventoryManager.Instance.SaveData();
            //保存任务数据
            QuestManager.Instance.SaveQuestSystem();
            yield return SceneManager.LoadSceneAsync(sceneName); //当另一个场景异步加载完毕，当前场景的一切都会消失
            
            var destTransform = GetTransitionDestination(destinationTag).transform;
            yield return Instantiate(playerPrefab, destTransform.position, destTransform.rotation); //等玩家生成完毕才会执行下面的代码
            SaveManager.Instance.LoadData();
        }
        else
        {
            playerPrefab = GameManager.Instance.playerStats.gameObject;
            var destination = GetTransitionDestination(destinationTag).transform;
            playerPrefab.transform.SetPositionAndRotation(destination.position,destination.rotation);
            yield return null;
        }
    }

    /// <summary>
    /// 找到指定destinationTag的传送门
    /// </summary>
    /// <param name="destinationTag"></param>
    private TransitionDestination GetTransitionDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrance = FindObjectsOfType<TransitionDestination>();

        foreach (var transitionPoint in entrance)
        {
            if (transitionPoint.destinationTag==destinationTag)
            {
                return transitionPoint;
            }
        }

        return null;
    }

    public void TransitionToLoadLevel()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("First"));
    }

    /// <summary>
    /// 在菜单中开始新游戏时加载场景使用
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadLevel(string sceneName)
    {
        FadeCanvas fadeCanvas = Instantiate(fadeCanvasPrefab);
        if (!sceneName.Equals(""))
        {
            yield return StartCoroutine(fadeCanvas.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(sceneName);
            
            var entrance = GameManager.Instance.GetEntrance();
            yield return Instantiate(playerPrefab, entrance.position, entrance.rotation);
            
            SaveManager.Instance.SavaData();
            InventoryManager.Instance.SaveData();
            yield return StartCoroutine(fadeCanvas.FadeIn(2f));
        }
    }

    public void TransitionToMainMenu()
    {
        StartCoroutine(LoadMain());
    }

    /// <summary>
    /// 淡入淡出回到主菜单
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadMain()
    {
        FadeCanvas fadeCanvas = Instantiate(fadeCanvasPrefab);
        yield return StartCoroutine(fadeCanvas.FadeOut(2f));
        SaveManager.Instance.SavaData();
        //保存背包数据
        InventoryManager.Instance.SaveData();
        //保存任务数据
        QuestManager.Instance.SaveQuestSystem();
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fadeCanvas.FadeIn(2f));
    }
    
    public void EndNotify()
    {
        //GameManager会循环执行此方法，必须加条件使其无法循环执行
        if (_fadeFinished)
        {
            _fadeFinished = false;
            //人物死亡，删除所有数据，返回主菜单
            PlayerPrefs.DeleteAll();
            StartCoroutine(LoadMain());
        }
    }
}

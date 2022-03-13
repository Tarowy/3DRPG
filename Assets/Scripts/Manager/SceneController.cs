using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public GameObject player;
    
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
                player.GetComponent<NavMeshAgent>().isStopped = true;
                break;
            case TransitionPoint.TransitionType.DifferentScene:
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
        player = GameManager.Instance.playerStats.gameObject;
        var destination = GetTransitionDestination(destinationTag).transform;
        player.transform.SetPositionAndRotation(destination.position,destination.rotation);
        yield return null;
    }

    /// <summary>
    /// 找到指定destinationTag的传送门
    /// </summary>
    /// <param name="destinationTag"></param>
    private TransitionDestination GetTransitionDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrance = GameObject.FindObjectsOfType<TransitionDestination>();

        foreach (var transitionPoint in entrance)
        {
            if (transitionPoint.destinationTag==destinationTag)
            {
                return transitionPoint;
            }
        }

        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")] 
    public string transitionName;
    
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;

    private bool _canTrans;

    private void Update()
    {
        if (_canTrans && Input.GetKeyDown(KeyCode.E))
        {
            //传送玩家
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTrans = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTrans = false;
        }
    }
}

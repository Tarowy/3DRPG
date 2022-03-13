using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranstionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")] 
    public string transitionName;
    
    public TransitionType transitionType;
    public TranstionDestination.DestinationTag destinationTag;

    private bool _canTrans;

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

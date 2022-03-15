using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratManager : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject saveManger;
    public GameObject sceneManger;
    public GameObject mouseManger;

    public GameObject player;

    private void Awake()
    {
        Instantiate(gameManager);
        Instantiate(mouseManger);
        Instantiate(sceneManger);
        Instantiate(saveManger);
    }

    private void Start()
    {
        Instantiate(player);
    }
}

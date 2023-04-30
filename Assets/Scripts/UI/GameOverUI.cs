using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : Singleton<GameOverUI>
{
    [SerializeField] private RectTransform container;

    private void Awake()
    {
        container.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        LevelScenario.Instance.OnReturnedToLaunch += ShowWin;
    }

    public void ShowWin()
    {
        container.gameObject.SetActive(true);
    }
}
